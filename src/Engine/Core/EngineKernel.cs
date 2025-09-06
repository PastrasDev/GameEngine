﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine.Core;

public sealed class PhaseBarrier
{
    public readonly ManualResetEventSlim MainInitDone = new(false);
    public readonly ManualResetEventSlim GameInitDone = new(false);
    public readonly ManualResetEventSlim RenderInitDone = new(false);

    public readonly ManualResetEventSlim MainStartDone = new(false);
    public readonly ManualResetEventSlim GameStartDone = new(false);
    public readonly ManualResetEventSlim RenderStartDone = new(false);
}

public abstract class EngineKernel
{
    protected const float _fixedDt = 1f / 120f;
    protected const float _maxFrameDt = 0.25f;
    protected const int _maxFixedStepsPerFrame = 8;
    
    public Thread Thread { get; protected set; } = null!;
    public volatile ExitCode Code;
    
    private readonly int _initCount;
    private readonly bool _launch;
    private readonly Threads _affinity;
    private readonly EngineContext _context = null!;
    
    public required Threads Affinity { get => _affinity; init { _affinity = value; _initCount++; TryLaunch(); } }
    public required EngineContext Context { get => _context; init { _context = value; _initCount++; TryLaunch(); } }
    public required bool Enabled { init { _launch = value; TryLaunch(); } }
    
    private (int start, int len) _initR;
    private (int start, int len) _startR;
    private (int start, int len) _fixedR;
    private (int start, int len) _updateR;
    private (int start, int len) _lateR;
    private (int start, int len) _shutR;
    
    private readonly List<object> _modulesInit  = [];
    private readonly List<object> _modulesStart = [];
    private readonly List<object> _modulesFixed = [];
    private readonly List<object> _modulesUpdate= [];
    private readonly List<object> _modulesLate  = [];
    private readonly List<object> _modulesShut  = [];
    
    private readonly List<nint> _functionsInit   = [];
    private readonly List<nint> _functionsStart  = [];
    private readonly List<nint> _functionsFixed  = [];
    private readonly List<nint> _functionsUpdate = [];
    private readonly List<nint> _functionsLate   = [];
    private readonly List<nint> _functionsShut   = [];
    
    private readonly List<nint> _enabledInit   = [];
    private readonly List<nint> _enabledStart  = [];
    private readonly List<nint> _enabledFixed  = [];
    private readonly List<nint> _enabledUpdate = [];
    private readonly List<nint> _enabledLate   = [];
    private readonly List<nint> _enabledShut   = [];
    
    private object[] _modules = [];
    private nint[] _fns  = [];
    private nint[] _ens = [];
    
    protected abstract void Run();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Initialize() => Exec(_initR);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Start() => Exec(_startR);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void FixedUpdate() => Exec(_fixedR);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Update() => Exec(_updateR);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void LateUpdate() => Exec(_lateR);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Shutdown() => Exec(_shutR, true, true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void Exec((int start, int len) r, bool reverse = false, bool ignore = false)
    {
        var modules = _modules;
        var fns  = _fns;
        var ens  = _ens;
        
        if (!reverse)
        {
            for (int i = r.start, end = r.start + r.len; i < end; i++)
            {
                var m = modules[i];
                if (!ignore && !((delegate* managed<object, bool>)ens[i])(m)) continue;
                ((delegate* managed<object, void>)fns[i])(m);
            }
        }
        else
        {
            for (int i = r.start + r.len - 1; i >= r.start; i--)
            {
                var m = modules[i];
                if (!ignore && !((delegate* managed<object, bool>)ens[i])(m)) continue;
                ((delegate* managed<object, void>)fns[i])(m);
            }
        }
    }
    
    private void TryLaunch()
    {
        if (!_launch || _initCount < 2)
            return;
        
        switch (Affinity)
        {
            case Threads.Main:
            {
                Run();
                break;
            }
            case Threads.Game:
            case Threads.Render:
            {
                Thread = new Thread(Run)
                {
                    Name = Affinity.ToString(),
                    IsBackground = true,
                };
                Thread.Start();

                break;
            }
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(Affinity), Affinity, "Invalid thread affinity for kernel.");
            }
        }
    }

    public unsafe void Add<T>(T m) where T : EngineModule<T>
    {
        m.Context = require(Context);
        
        var en = EngineModule<T>.__EnabledPtr;
        
        foreach (var pf in EngineModule<T>.Descriptor.Overrides)
        {
            switch (pf.Phase)
            {
                case Phase.Init:     _modulesInit.Add(m);  _functionsInit.Add((nint)pf.Fn);  _enabledInit.Add(en);  break;
                case Phase.Start:    _modulesStart.Add(m); _functionsStart.Add((nint)pf.Fn); _enabledStart.Add(en); break;
                case Phase.Fixed:    _modulesFixed.Add(m); _functionsFixed.Add((nint)pf.Fn); _enabledFixed.Add(en); break;
                case Phase.Update:   _modulesUpdate.Add(m);_functionsUpdate.Add((nint)pf.Fn);_enabledUpdate.Add(en);break;
                case Phase.Late:     _modulesLate.Add(m);  _functionsLate.Add((nint)pf.Fn);  _enabledLate.Add(en);  break;
                case Phase.Shutdown: _modulesShut.Add(m);  _functionsShut.Add((nint)pf.Fn);  _enabledShut.Add(en);  break;
            }
        }
    }
    
    internal void LoadModules(IReadOnlyList<ModuleDescriptor> descriptors)
    {
        var addMI = typeof(EngineKernel).GetMethod(nameof(Add), BindingFlags.Instance | BindingFlags.Public)!;
        foreach (var desc in descriptors)
        {
            var type = desc.Type;
            var addT = addMI.MakeGenericMethod(type);
            var instance = require(Activator.CreateInstance(type));
            
            foreach (var pf in desc.Overrides)
            {
                if (pf.Phase != Phase.Load) continue;
                unsafe { pf.Fn(instance); }
                break;
            }
            
            addT.Invoke(this, [instance]);
        }
    }
    
    public void PostLoadModules()
    {
        int total = _modulesInit.Count + _modulesStart.Count + _modulesFixed.Count + _modulesUpdate.Count + _modulesLate.Count + _modulesShut.Count;

        _modules = new object[total];
        _fns  = new nint[total];
        _ens  = new nint[total];

        int o = 0;
        _initR   = CopyRange(_modulesInit,  _functionsInit,  _enabledInit,  ref o);
        _startR  = CopyRange(_modulesStart, _functionsStart, _enabledStart, ref o);
        _fixedR  = CopyRange(_modulesFixed, _functionsFixed, _enabledFixed, ref o);
        _updateR = CopyRange(_modulesUpdate,_functionsUpdate,_enabledUpdate,ref o);
        _lateR   = CopyRange(_modulesLate,  _functionsLate,  _enabledLate,  ref o);
        _shutR   = CopyRange(_modulesShut,  _functionsShut,  _enabledShut,  ref o);
        
        _modulesInit.Clear();
        _modulesStart.Clear();
        _modulesFixed.Clear();
        _modulesUpdate.Clear();
        _modulesLate.Clear();
        _modulesShut.Clear();
        
        _functionsInit.Clear();
        _functionsStart.Clear();
        _functionsFixed.Clear();
        _functionsUpdate.Clear();
        _functionsLate.Clear();
        _functionsShut.Clear();
        
        _enabledInit.Clear();
        _enabledStart.Clear();
        _enabledFixed.Clear();
        _enabledUpdate.Clear();
        _enabledLate.Clear();
        _enabledShut.Clear();
        return;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        (int start, int len) CopyRange(List<object> mods, List<nint> fns, List<nint> ens, ref int offset)
        {
            int len = mods.Count;
            for (int i = 0; i < len; i++)
            {
                _modules[offset + i] = mods[i];
                _fns[offset + i]     = fns[i];
                _ens[offset + i]     = ens[i];
            }
            var r = (offset, len);
            offset += len;
            return r;
        }
    }
    
    
}

public sealed class MainKernel : EngineKernel
{
    protected override void Run()
    {
        try
        {
            Thread = Thread.CurrentThread;
            
            ModuleLoader.Load(this);
            
            var barrier = require(Context.Registry.Get<PhaseBarrier>());
            var channel = require(Context.Registry.Get<IMainChannels>());
            var ct = Context.TokenSrc.Token;
            
            Initialize();
            barrier.MainInitDone.Set();
            
            Start();
            barrier.MainStartDone.Set();
            
            var sw = Stopwatch.StartNew();
            long frameId = 0;
            double last = 0;
            
            while (!ct.IsCancellationRequested)
            {
                Update();
                LateUpdate();

                var now = sw.Elapsed.TotalSeconds;
                var raw = (float)(now - last);
                last = now;
                var dt = clamp(raw, 0f, _maxFrameDt);

                channel.FrameWriter.Write(new FrameStart(frameId, dt), ct);
                frameId++;
            }

            Code = ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            Code = ExitCode.Canceled;
        }
        catch (Exception)
        {
            Code = ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}

public sealed class GameKernel : EngineKernel
{
    protected override void Run()
    {
        try
        {
            var barrier = require(Context.Registry.Get<PhaseBarrier>());
            var channel = require(Context.Registry.Get<IGameChannels>());
            var ct = Context.TokenSrc.Token;
            
            barrier.MainInitDone.Wait(ct);
            
            ModuleLoader.Load(this);
            
            Initialize();
            barrier.GameInitDone.Set();
                
            barrier.MainStartDone.Wait(ct);
            Start();
            barrier.GameStartDone.Set();

            float accumulator = 0f;
            int simFrame = 0;
            
            while (!ct.IsCancellationRequested)
            {
                var start = channel.FrameReader.Read(ct);

                var dt = clamp(start.Dt, 0f, _maxFrameDt);
                accumulator += dt;

                int steps = 0;
                while (accumulator >= _fixedDt && steps < _maxFixedStepsPerFrame)
                {
                    Context.DeltaTime = _fixedDt;
                    FixedUpdate();
                    accumulator -= _fixedDt;
                    simFrame++;
                    steps++;
                }

                Context.DeltaTime = dt;
                Update();
                LateUpdate();

                channel.SceneWriter.Publish(new SceneView(simFrame, clamp(accumulator / _fixedDt, 0f, 1f)));
            }
                
            Code = ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            Code = ExitCode.Canceled;
        }
        catch (Exception)
        {
            Code = ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}

public sealed class RenderKernel : EngineKernel
{
    protected override void Run()
    {
        try
        {
            var barrier = require(Context.Registry.Get<PhaseBarrier>());
            var channel = require(Context.Registry.Get<IRenderChannels>());
            var ct = Context.TokenSrc.Token;
            
            barrier.GameInitDone.Wait(ct);
            
            ModuleLoader.Load(this);
            
            Initialize();
            barrier.RenderInitDone.Set();
                
            barrier.GameStartDone.Wait(ct);
            Start();
            barrier.RenderStartDone.Set();
            
            while (!ct.IsCancellationRequested)
            {
                channel.SceneReader.Read();

                Update();
                LateUpdate();
            }
                
            Code = ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            Code = ExitCode.Canceled;
        }
        catch (Exception)
        {
            Code = ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}
using System.Runtime.Versioning;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Dxgi.Common;
using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Debug;
using Engine.Graphics.DXGI;
using Engine.Graphics.Interop;
using Engine.Platform.Windows;
using static Engine.Graphics.D3D11.Abstractions;

namespace Engine.Graphics.D3D11;

[Discoverable]
internal sealed class Renderer : Module<Renderer>, IConfigure
{
	private ComPtr<ID3D11Device5> _device = ComPtr<ID3D11Device5>.Null;
	private ComPtr<ID3D11DeviceContext4> _context = ComPtr<ID3D11DeviceContext4>.Null;
	private D3D_FEATURE_LEVEL _featureLevel;

	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
		Affinity = Affinity.Render;
	}

	public override void Load()
	{
		base.Load();
	}

	[SupportedOSPlatform("windows10.0.17763")]
	public override void Initialize()
	{
		base.Initialize();

		Span<D3D_FEATURE_LEVEL> levels =
		[
			D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1,
			D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
			D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
			D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0
		];

		var flags = D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT;

		#if DEBUG
		flags |= D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
		(_device, _context, _featureLevel) = D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, default, flags, levels);
		using (var debug = _device.QueryInterface<ID3D11Debug>())
		{
			Log.Debug($"D3D11 Debug interface acquired.");
			debug.ReportLiveDeviceObjects(D3D11_RLDO_FLAGS.D3D11_RLDO_SUMMARY).ThrowOnFailure();
		}
		#else
		(_device, _context, _featureLevel) = D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, default, flags, levels);
		#endif

		using var dxgiDevice4 = _device.QueryInterface<IDXGIDevice4>();
		using var dxgiAdapter4 = dxgiDevice4.GetAdapter<IDXGIAdapter4>();
		using var dxgiFactory = dxgiAdapter4.GetParent<IDXGIFactory7>();

		var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1
		{
			Width = 1280,
			Height = 720,
			Format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
			Stereo = false,
			SampleDesc = new DXGI_SAMPLE_DESC
			{
				Count = 1,
				Quality = 0
			},
			BufferUsage = DXGI_USAGE.DXGI_USAGE_RENDER_TARGET_OUTPUT,
			BufferCount = 2,
			Scaling = DXGI_SCALING.DXGI_SCALING_STRETCH,
			SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_DISCARD,
			AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_UNSPECIFIED,
			Flags = 0
		};

		var swapChainFullscreenDesc = new DXGI_SWAP_CHAIN_FULLSCREEN_DESC
		{
			RefreshRate = new DXGI_RATIONAL { Numerator = 60, Denominator = 1 },
			ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER.DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED,
			Scaling = DXGI_MODE_SCALING.DXGI_MODE_SCALING_UNSPECIFIED,
			Windowed = true
		};


		//using var swapChain = dxgiFactory.CreateSwapChainForHwnd(_device, hwnd, swapChainDesc, swapChainFullscreenDesc);
	}

	public override void Start()
	{
		base.Start();

	}

	public override void Update()
	{
		base.Update();
	}

	public override void PostUpdate()
	{
		base.PostUpdate();
	}

	public override void Shutdown()
	{
		base.Shutdown();

		_context.Dispose();
		_device.Dispose();
	}
}
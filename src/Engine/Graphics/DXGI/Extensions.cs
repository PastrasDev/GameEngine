using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.System.Com;
using Engine.Graphics.Interop;

namespace Engine.Graphics.DXGI
{
	public static class Extensions
	{
		private const int EPointer = unchecked((int)0x80004003);

		public static unsafe ComPtr<T> GetAdapter<T>(this ComPtr<IDXGIDevice4> dxgiDevice, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0) where T : unmanaged
		{
			if (dxgiDevice.IsNull) throw new ArgumentNullException($"DXGI device ComPtr is null at {caller} ({file}:{line})");
			IDXGIAdapter* raw = null;
			((IDXGIDevice4*)dxgiDevice.Ptr)->GetAdapter(&raw).ThrowOnFailure();
			if (raw is null) throw new InvalidOperationException($"GetAdapter returned null at {caller} ({file}:{line})");
			using var baseAdapter = new ComPtr<IDXGIAdapter>((nint)raw);
			return baseAdapter.QueryInterface<T>();
		}

		public static unsafe ComPtr<T> GetParent<T>(this ComPtr<IDXGIAdapter4> dxgiObject, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0) where T : unmanaged
		{
			if (dxgiObject.IsNull) throw new ArgumentNullException($"DXGI object ComPtr is null at {caller} ({file}:{line})");
			var iid = typeof(T).GUID;
			T* raw = null;
			((IDXGIAdapter4*)dxgiObject.Ptr)->GetParent(&iid, (void**)&raw).ThrowOnFailure();
			return raw is null ? throw new InvalidOperationException($"GetParent<{typeof(T).Name}> returned null at {caller} ({file}:{line})") : new ComPtr<T>((nint)raw);
		}

		public static unsafe DXGI_ADAPTER_DESC GetDesc(this ComPtr<IDXGIAdapter> dxgiAdapter, [CallerArgumentExpression(nameof(dxgiAdapter))] string? expr = null, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
		{
			if (dxgiAdapter.IsNull) throw new ArgumentNullException(expr ?? nameof(dxgiAdapter), $"DXGI adapter ComPtr is null at {caller} ({file}:{line})");
			DXGI_ADAPTER_DESC desc;
			((IDXGIAdapter*)dxgiAdapter.Ptr)->GetDesc(&desc).ThrowOnFailure();
			return desc;
		}

		[SupportedOSPlatform("windows10.0.17763")]
		public static unsafe ComPtr<IDXGISwapChain4> CreateSwapChainForHwnd(this ComPtr<IDXGIFactory7> factory7, ComPtr<ID3D11Device5> device, HWND hWnd, in DXGI_SWAP_CHAIN_DESC1 desc, in DXGI_SWAP_CHAIN_FULLSCREEN_DESC fullscreenDesc, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
		{
			if (factory7.IsNull) throw new ArgumentNullException($"DXGI factory7 ComPtr is null at {caller} ({file}:{line})");
			if (device.IsNull) throw new ArgumentNullException(nameof(device), $"D3D11 device ComPtr is null at {caller} ({file}:{line})");

			DXGI_SWAP_CHAIN_DESC1 copyDesc = desc;
			DXGI_SWAP_CHAIN_FULLSCREEN_DESC copyFullscreenDesc = fullscreenDesc;
			IDXGISwapChain1* raw = null;

			((IDXGIFactory7*)factory7.Ptr)->CreateSwapChainForHwnd((IUnknown*)device.Ptr, hWnd, &copyDesc, &copyFullscreenDesc, null, &raw).ThrowOnFailure();
			if (raw is null) throw new InvalidOperationException($"CreateSwapChainForHwnd returned null at {caller} ({file}:{line})");
			return new ComPtr<IDXGISwapChain4>((nint)raw).QueryInterface<IDXGISwapChain4>();
		}


	}
}
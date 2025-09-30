using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Engine.Graphics.Interop;

namespace Engine.Graphics.D3D11
{
	public static class Abstractions
	{
		[SupportedOSPlatform("windows8.0")]
		public static unsafe (ComPtr<ID3D11Device5> device, ComPtr<ID3D11DeviceContext4> context, D3D_FEATURE_LEVEL featureLevel) D3D11CreateDevice(ComPtr<IDXGIAdapter>? pAdapter, D3D_DRIVER_TYPE driverType, HMODULE software, D3D11_CREATE_DEVICE_FLAG flags, ReadOnlySpan<D3D_FEATURE_LEVEL> featureLevels, [CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0)
		{
			ID3D11Device* device = null;
			ID3D11DeviceContext* context = null;
			D3D_FEATURE_LEVEL level = default;

			fixed (D3D_FEATURE_LEVEL* pLevels = featureLevels)
			{
				var hr = PInvoke.D3D11CreateDevice((IDXGIAdapter*)(pAdapter?.Ptr ?? 0), driverType, software, flags, featureLevels.Length > 0 ? pLevels : null, (uint)featureLevels.Length, PInvoke.D3D11_SDK_VERSION, &device, &level, &context);
				if (hr.Failed) throw new InvalidOperationException($"{nameof(D3D11CreateDevice)} failed with HRESULT {hr} at {caller} ({file}:{line})");
				return (new ComPtr<ID3D11Device5>((nint)device), new ComPtr<ID3D11DeviceContext4>((nint)context), level);
			}
		}
	}

	public static class Extensions
	{
		private const int EPointer = unchecked((int)0x80004003);

		public static unsafe HRESULT ReportLiveDeviceObjects(this ComPtr<ID3D11Debug> debug, D3D11_RLDO_FLAGS flags) => debug.IsNull ? new HRESULT(EPointer) : ((ID3D11Debug*)debug.Ptr)->ReportLiveDeviceObjects(flags);
	}
}
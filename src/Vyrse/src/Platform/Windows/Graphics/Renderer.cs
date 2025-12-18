// using System.Runtime.Versioning;
// using Vyrse.Core;
//
// namespace Vyrse.Platform.Windows.Graphics;
//
// [Discoverable]
// internal sealed class Renderer : Module<Renderer>, IModule
// {
// 	// private D3D11Device5 _device = null!;
// 	// private D3D11DeviceContext4 _context = null!;
// 	// private DXGISwapChain4 _swapChain = null!;
// 	// private D3D11RenderTargetView1 _renderTargetView = null!;
// 	// private D3D11DepthStencilView _depthStencilView = null!;
// 	// private D3D11RasterizerState2 _rasterizerState = null!;
// 	//
// 	// private D3D11Buffer _matrixBuffer = null!;
// 	//
// 	// private D3D_FEATURE_LEVEL _featureLevel;
//
// 	public static Metadata[] Dependencies => [];
//
// 	public void Load()
// 	{
//
// 	}
//
// 	[SupportedOSPlatform("windows10.0.17763")]
// 	public void Initialize()
// 	{
// 		// {
// 		// 	ReadOnlySpan<D3D_FEATURE_LEVEL> levels =
// 		// 	[
// 		// 		D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1,
// 		// 		D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
// 		// 		D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
// 		// 		D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0
// 		// 	];
// 		//
// 		// 	var flags = D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT;
// 		// 	#if DEBUG
// 		// 	flags |= D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
// 		// 	#endif
// 		//
// 		// 	(var device, var context, _featureLevel) = D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, default, flags, levels);
// 		//
// 		// 	_device = device.Query<D3D11Device5>();
// 		// 	_context = context.Query<D3D11DeviceContext4>();
// 		// }
// 		//
// 		// {
// 		// 	using var dxgiDevice = _device.Query<DXGIDevice4>();
// 		// 	using var dxgiAdapter = dxgiDevice.GetAdapter<DXGIAdapter4>();
// 		// 	using var dxgiFactory = dxgiAdapter.GetParent<DXGIFactory7>();
// 		//
// 		// 	var (hwnd, windowed, width, height) = Receive<ValueTuple<HWND, bool, int, int>>();
// 		//
// 		// 	var swapChainDesc1 = new DXGI_SWAP_CHAIN_DESC1
// 		// 	{
// 		// 		Width = (uint)width,
// 		// 		Height = (uint)height,
// 		// 		Format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
// 		// 		Stereo = false,
// 		// 		SampleDesc = new DXGI_SAMPLE_DESC
// 		// 		{
// 		// 			Count = 1,
// 		// 			Quality = 0
// 		// 		},
// 		// 		BufferUsage = DXGI_USAGE.DXGI_USAGE_RENDER_TARGET_OUTPUT,
// 		// 		BufferCount = 2,
// 		// 		Scaling = DXGI_SCALING.DXGI_SCALING_STRETCH,
// 		// 		SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_DISCARD,
// 		// 		AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_UNSPECIFIED,
// 		// 		Flags = 0
// 		// 	};
// 		//
// 		// 	var swapChainFullscreenDesc = new DXGI_SWAP_CHAIN_FULLSCREEN_DESC
// 		// 	{
// 		// 		RefreshRate = new DXGI_RATIONAL
// 		// 		{
// 		// 			Numerator = 60,
// 		// 			Denominator = 1
// 		// 		},
// 		// 		ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER.DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED,
// 		// 		Scaling = DXGI_MODE_SCALING.DXGI_MODE_SCALING_UNSPECIFIED,
// 		// 		Windowed = windowed
// 		// 	};
// 		//
// 		// 	_swapChain = dxgiFactory.CreateSwapChainForHwnd(_device, hwnd, swapChainDesc1, swapChainFullscreenDesc).Query<DXGISwapChain4>();
// 		// }
// 		//
// 		// {
// 		// 	using var backBuffer = _swapChain.GetBuffer<D3D11Texture2D1>();
// 		// 	_renderTargetView = _device.CreateRenderTargetView1(backBuffer);
// 		// }
// 		//
// 		// {
// 		// 	var swapChainDesc1 = _swapChain.GetDesc1();
// 		// 	var depthStencilDesc = new D3D11_TEXTURE2D_DESC1
// 		// 	{
// 		// 		Width = swapChainDesc1.Width,
// 		// 		Height = swapChainDesc1.Height,
// 		// 		MipLevels = 1,
// 		// 		ArraySize = 1,
// 		// 		Format = DXGI_FORMAT.DXGI_FORMAT_D24_UNORM_S8_UINT,
// 		// 		SampleDesc = new DXGI_SAMPLE_DESC { Count = 1, Quality = 0 },
// 		// 		Usage = D3D11_USAGE.D3D11_USAGE_DEFAULT,
// 		// 		BindFlags = D3D11_BIND_FLAG.D3D11_BIND_DEPTH_STENCIL,
// 		// 		CPUAccessFlags = 0,
// 		// 		MiscFlags = 0,
// 		// 		TextureLayout = D3D11_TEXTURE_LAYOUT.D3D11_TEXTURE_LAYOUT_UNDEFINED
// 		// 	};
// 		//
// 		// 	using var depthStencilBuffer = _device.CreateTexture2D1(depthStencilDesc);
// 		//
// 		// 	var depthStencilViewDesc = new D3D11_DEPTH_STENCIL_VIEW_DESC
// 		// 	{
// 		// 		Format =  DXGI_FORMAT.DXGI_FORMAT_D24_UNORM_S8_UINT,
// 		// 		ViewDimension = D3D11_DSV_DIMENSION.D3D11_DSV_DIMENSION_TEXTURE2D,
// 		// 		Flags = 0,
// 		// 		Anonymous = new D3D11_DEPTH_STENCIL_VIEW_DESC._Anonymous_e__Union
// 		// 		{
// 		// 			Texture2D = new D3D11_TEX2D_DSV
// 		// 			{
// 		// 				MipSlice = 0
// 		// 			}
// 		// 		}
// 		// 	};
// 		//
// 		// 	_depthStencilView = _device.CreateDepthStencilView(depthStencilBuffer, depthStencilViewDesc);
// 		// }
// 		//
// 		// {
// 		// 	var matrixBufferDesc = new D3D11_BUFFER_DESC
// 		// 	{
// 		// 		Usage = D3D11_USAGE.D3D11_USAGE_DEFAULT,
// 		// 		ByteWidth = (uint)sizeof(float) * 16 * 3,
// 		// 		BindFlags = D3D11_BIND_FLAG.D3D11_BIND_CONSTANT_BUFFER,
// 		// 		CPUAccessFlags = 0,
// 		// 		MiscFlags = 0,
// 		// 		StructureByteStride = 0
// 		// 	};
// 		//
// 		// 	_matrixBuffer = _device.CreateBuffer(matrixBufferDesc);
// 		// }
// 		//
// 		// {
// 		// 	var rasterizerDesc = new D3D11_RASTERIZER_DESC2
// 		// 	{
// 		// 		FillMode = D3D11_FILL_MODE.D3D11_FILL_SOLID,
// 		// 		CullMode = D3D11_CULL_MODE.D3D11_CULL_BACK,
// 		// 		FrontCounterClockwise = false,
// 		// 		DepthBias = 0,
// 		// 		DepthBiasClamp = 0.0f,
// 		// 		SlopeScaledDepthBias = 0.0f,
// 		// 		DepthClipEnable = true,
// 		// 		ScissorEnable = false,
// 		// 		MultisampleEnable = false,
// 		// 		AntialiasedLineEnable = false,
// 		// 		ForcedSampleCount = 0,
// 		// 		ConservativeRaster = D3D11_CONSERVATIVE_RASTERIZATION_MODE.D3D11_CONSERVATIVE_RASTERIZATION_MODE_OFF
// 		// 	};
// 		//
// 		// 	_rasterizerState = _device.CreateRasterizerState2(rasterizerDesc);
// 		// }
// 	}
//
// 	public void Start()
// 	{
//
// 	}
//
// 	public void Update()
// 	{
//
// 	}
//
// 	public void PostUpdate()
// 	{
//
// 	}
//
// 	public void Shutdown()
// 	{
// 		// _rasterizerState.Dispose();
// 		// _matrixBuffer.Dispose();
// 		// _depthStencilView.Dispose();
// 		// _renderTargetView.Dispose();
// 		// _swapChain.Dispose();
// 		// _context.Dispose();
// 		// _device.Dispose();
// 	}
// }
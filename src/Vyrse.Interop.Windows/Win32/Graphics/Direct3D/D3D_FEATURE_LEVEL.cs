namespace Windows.Win32.Graphics.Direct3D;

public enum D3D_FEATURE_LEVEL
{
	D3D_FEATURE_LEVEL_1_0_GENERIC = 256,
	/// <summary>Allows Microsoft Compute Driver Model (MCDM) devices to be used, or more feature-rich devices (such as traditional GPUs) that support a superset of the functionality. MCDM is the overall driver model for compute-only; it's a scaled-down peer of the larger scoped Windows Device Driver Model (WDDM).</summary>
	D3D_FEATURE_LEVEL_1_0_CORE = 4096,
	/// <summary>Targets features supported by [feature level](/windows/desktop/direct3d11/overviews-direct3d-11-devices-downlevel-intro) 9.1, including shader model 2.</summary>
	D3D_FEATURE_LEVEL_9_1 = 37120,
	/// <summary>Targets features supported by [feature level](/windows/desktop/direct3d11/overviews-direct3d-11-devices-downlevel-intro) 9.2, including shader model 2.</summary>
	D3D_FEATURE_LEVEL_9_2 = 37376,
	/// <summary>Targets features supported by [feature level](/windows/desktop/direct3d11/overviews-direct3d-11-devices-downlevel-intro) 9.3, including shader model 2.0b.</summary>
	D3D_FEATURE_LEVEL_9_3 = 37632,
	/// <summary>Targets features supported by Direct3D 10.0, including shader model 4.</summary>
	D3D_FEATURE_LEVEL_10_0 = 40960,
	/// <summary>Targets features supported by Direct3D 10.1, including shader model 4.</summary>
	D3D_FEATURE_LEVEL_10_1 = 41216,
	/// <summary>Targets features supported by Direct3D 11.0, including shader model 5.</summary>
	D3D_FEATURE_LEVEL_11_0 = 45056,
	/// <summary>Targets features supported by Direct3D 11.1, including shader model 5 and logical blend operations. This feature level requires a display driver that is at least implemented to WDDM for Windows 8 (WDDM 1.2).</summary>
	D3D_FEATURE_LEVEL_11_1 = 45312,
	/// <summary>Targets features supported by Direct3D 12.0, including shader model 5.</summary>
	D3D_FEATURE_LEVEL_12_0 = 49152,
	/// <summary>Targets features supported by Direct3D 12.1, including shader model 5.</summary>
	D3D_FEATURE_LEVEL_12_1 = 49408,
	/// <summary>Targets features supported by Direct3D 12.2, including shader model 6.5. For more information about feature level 12_2, see its [specification page](https://microsoft.github.io/DirectX-Specs/d3d/D3D12_FeatureLevel12_2.html). Feature level 12_2 is available in Windows SDK builds 20170 and later.</summary>
	D3D_FEATURE_LEVEL_12_2 = 49664,
}
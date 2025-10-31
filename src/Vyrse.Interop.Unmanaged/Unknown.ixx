export module Engine.Native.Unknown;

import <d3d11.h>;

#define VYRSEAPI extern "C" __declspec(dllexport)

VYRSEAPI HRESULT WINAPI IUnknown_QueryInterface(IUnknown* unknown, const IID& riid, void** ppvObject) noexcept { return unknown->QueryInterface(riid, ppvObject); }
VYRSEAPI ULONG WINAPI IUnknown_AddRef(IUnknown* unknown) noexcept { return unknown->AddRef(); }
VYRSEAPI ULONG WINAPI IUnknown_Release(IUnknown* unknown) noexcept { return unknown->Release(); }
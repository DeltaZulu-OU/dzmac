// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapterFactory.GetAll~System.Net.NetworkInformation.NetworkInterface[]")]
[assembly: SuppressMessage("Roslynator", "RCS1075:Avoid empty catch clause that catches System.Exception", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapterFactory.GetAll~System.Net.NetworkInformation.NetworkInterface[]")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.MacFormatter.Format(System.String,System.Object,System.IFormatProvider)~System.String")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.MacAddress.SafeStringFormat(System.IFormatProvider,System.String,System.String)~System.String")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.SafeConvertToInt(System.Object)~System.Int32")]
[assembly: SuppressMessage("Object lifecycle", "TI5113:Implement IDisposable if a class uses unmanaged resources, owns disposable objects or subscribes to other objects", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.Dispose(System.Boolean)")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~P:MacChanger.NetworkAdapter.IsDhcpEnabled")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.ExtractDeviceNumber~System.Int32")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.GetActiveMac~MacChanger.MacAddress")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.GetOriginalMacAddress~MacChanger.MacAddress")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.ExtractDnsConfig~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.ExtractGateway~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.NetworkAdapter.ExtractIPConfig~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.MacAddress.ConvertHexStringToByteArray(System.String)~System.Byte[]")]
[assembly: SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>", Scope = "member", Target = "~M:MacChanger.MacAddress.MergeByteArrays(System.Byte[],System.Byte[])~System.Byte[]")]
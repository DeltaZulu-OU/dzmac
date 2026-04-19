// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Object lifecycle", "TI5113:Implement IDisposable if a class uses unmanaged resources, owns disposable objects or subscribes to other objects", Justification = "Since the class is a Form, the disposal is handled in FormClosing event.", Scope = "type", Target = "~T:Dzmac.Forms.MainForm")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Forms.MainForm.VisitUrl(System.String)")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapterFactory.GetAll~System.Net.NetworkInformation.NetworkInterface[]")]
[assembly: SuppressMessage("Roslynator", "RCS1075:Avoid empty catch clause that catches System.Exception", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapterFactory.GetAll~System.Net.NetworkInformation.NetworkInterface[]")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.MacFormatter.Format(System.String,System.Object,System.IFormatProvider)~System.String")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.MacAddress.SafeStringFormat(System.IFormatProvider,System.String,System.String)~System.String")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.SafeConvertToInt(System.Object)~System.Int32")]
[assembly: SuppressMessage("Object lifecycle", "TI5113:Implement IDisposable if a class uses unmanaged resources, owns disposable objects or subscribes to other objects", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.Dispose(System.Boolean)")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~P:Dzmac.Core.NetworkAdapter.IsDhcpEnabled")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.ExtractDeviceNumber~System.Int32")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.GetActiveMac~Dzmac.Core.MacAddress")]
[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.GetOriginalMacAddress~Dzmac.Core.MacAddress")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.ExtractDnsConfig~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.ExtractGateway~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("General", "TI2107:Do not suppress compiler warnings in the code", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.NetworkAdapter.ExtractIPConfig~System.Management.ManagementBaseObject")]
[assembly: SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.MacAddress.ConvertHexStringToByteArray(System.String)~System.Byte[]")]
[assembly: SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>", Scope = "member", Target = "~M:Dzmac.Core.MacAddress.MergeByteArrays(System.Byte[],System.Byte[])~System.Byte[]")]
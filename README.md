# DZMAC

 In ALPHA stage.

![Sample](assets/window.png)

## Overview

DZMAC is a Windows desktop application for viewing network adapters and managing MAC address–related settings with a deliberately constrained scope. It is a reimplementation of TMAC but does **not** aim for feature parity.

The goal is to provide a focused, predictable, and maintainable application centered on core adapter management workflows.

## Status

This project is in **alpha**.

The focus is on stabilizing core functionality before expanding scope for DZMAC.

## Help

See [wiki](https://github.com/zbalkan/DZMAC/wiki/Help) for help.
Architecture decisions are documented in [`docs/adr`](docs/adr), including
[ADR 0001](docs/adr/0001-adapter-toggle-menu-only.md) for adapter toggle UX.

## Configuration migration note (v1 hardening P0)

Application-facing settings now use the canonical `Dzmac.*` prefix.
Legacy `DZMACLib.*` keys are still recognized for one release cycle as compatibility aliases.

Canonical keys:

- `Dzmac.VerboseDiagnostics`
- `Dzmac.OuiCachePath`
- `Dzmac.OuiEndpoint`
- `Dzmac.OuiDownloadTimeoutSeconds`
- `Dzmac.OuiDownloadRetryCount`
- `Dzmac.AdminOperationTimeoutSeconds`

## Features (current scope)

- Enumerate network adapters
- Display adapter and connection details
- Show original and active MAC addresses
- Change MAC address via registry override
- Restore original MAC address
- DHCPv4 operations (enable, disable, renew, release)
- Preserve important network configuration when disabling DHCPv4:
  - IP address
  - subnet mask
  - gateways
  - DNS servers
- Display adapter speed (with optional KB/s view)
- Export text report for currently displayed adapters
- Open Windows Network Connections

## How DZMAC differs from TMAC

DZMAC started as a reimplementation of TMAC, but it intentionally makes different
product and UX choices. The list below highlights the most important current
differences so expectations are clear.

### Physical adapters first (virtual adapters optional)
By default, DZMAC focuses the adapter list on likely physical adapters for a
cleaner day-to-day experience. Virtual/logical adapters can still be shown
through **Options → Show All Adapters**.

This also affects **File → Export Text Report**: the export contains exactly
what is currently shown in the adapter list. To export all adapters (including
virtual/logical), enable **Options → Show All Adapters** first.

### Adapter enable/disable is menu-driven
The **Enabled** checkbox in the adapter list is intentionally read-only and
serves as a status indicator only.

Adapter state changes are performed exclusively through
**Action → Enable Adapter** / **Action → Disable Adapter** so the flow can
consistently enforce confirmation dialogs, status-bar feedback, and diagnostics.

### Narrower feature scope, fewer bundled utilities
The following decisions define the current user-facing scope:

#### No DHCPv6
Only DHCPv4 is supported. DHCPv6 is intentionally out of scope.

#### No proxy management
Internet Explorer / system proxy configuration is not supported.

#### No auto-updater
The application does not include update infrastructure.

#### No system tray
The application is not a background utility:
- no system tray icon
- no tray animation

#### Preset file support is postponed
Preset files (`.tpf`) are planned but not part of the current milestone.  
As a result:
- no startup file association checks
- no preset import/export in current scope

#### Reduced "all-in-one" behavior
Unlike TMAC's broader utility surface, DZMAC keeps optional/auxiliary behavior
to a minimum and emphasizes explicit, focused actions in the main UI.

### DHCP disable behavior is safe-by-default
Disabling DHCPv4 preserves the current configuration instead of discarding it.

## Acknowledgements

Thanks to the following projects and resources:

- https://github.com/sietseringers/MACAddressTool  
  For internals and implementation ideas

- https://objectlistview.sourceforge.net/cs/index.html  
  For list-view handling

- https://web.archive.org/web/20161025183601/http://www.codeproject.com/Articles/15117/MAC-Address-Text-Box-and-Class  
  For MAC address textbox implementation reference

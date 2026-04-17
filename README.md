# dzmac

 In ALPHA stage.

![Sample](assets/window.png)

## Overview

dzmac is a Windows desktop application for viewing network adapters and managing MAC address–related settings with a deliberately constrained scope. It is a reimplementation of TMAC but does **not** aim for feature parity.

The goal is to provide a focused, predictable, and maintainable application centered on core adapter management workflows.

## Status

This project is in **alpha**.

The focus is on stabilizing core functionality before expanding scope for dzmac.

## Help

See [wiki](https://github.com/zbalkan/dzmac/wiki/Help) for help.

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
- Open Windows Network Connections

## Deviations from TMAC

The following decisions define the current user-facing scope:

### No DHCPv6
Only DHCPv4 is supported. DHCPv6 is intentionally out of scope.

### No proxy management
Internet Explorer / system proxy configuration is not supported.

### No auto-updater
The application does not include update infrastructure.

### No system tray
The application is not a background utility:
- no system tray icon
- no tray animation

### Preset file support is postponed
Preset files (`.tpf`) are planned but not part of the current milestone.  
As a result:
- no startup file association checks
- no preset import/export in current scope

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



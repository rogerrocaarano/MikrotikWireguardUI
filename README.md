## Project description

MikrotikWireguardUI (a.k.a. MWUI) provides an easy to use graphic interface for managing multiple Mikrotik's Wireguard
servers using Rest API calls.

### Objectives

- [X] Request all Wireguard Interfaces.
- [X] Create new Wireguard Interfaces.
- [ ] Request all Wireguard Peers.
- [ ] Create new Wireguard Peers.
- [X] Handle multiple Mikrotik routers.
- [ ] Require Authentication.
- [ ] Allow users to create new peers for personal devices.
- [ ] Generate config files for clients.
- [ ] Generate config QR for clients.

## Setting-up development environment

The project requires dotNET Framework 7.

- Clone [mikrotikWireguardHandler](https://github.com/rogerrocaarano/mikrotikWireguardHandler) and this repo to the 
same location.

```
mkdir mwui
cd mwui
git clone https://github.com/rogerrocaarano/mikrotikWireguardHandler.git
git clone https://github.com/rogerrocaarano/MikrotikWireguardUI.git
```
- Apply database migrations generated from Entity Framework.
```
cd MikrotikWireguardUI
dotnet ef database update
```
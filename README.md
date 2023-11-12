# Yggdrasil.Net - minecraft auth server
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/mixa3607/Yggdrasil.Net/push.yml?branch=master&style=flat-square)
![GitHub](https://img.shields.io/github/license/mixa3607/Yggdrasil.Net?style=flat-square)

## Setup server and client to use custom auth
- Client side [HMCLauncher](https://github.com/huanghongxun/HMCL)
- Server side [AuthLib injector](https://github.com/yushijinhun/authlib-injector)

## X509 certs for signing generation
```sh
cd /files/certs
openssl genrsa 4096 > private.pem
openssl req -x509 -new -key private.pem -out public.pem
openssl pkcs12 -export -in public.pem -inkey private.pem -out cert.pfx
```

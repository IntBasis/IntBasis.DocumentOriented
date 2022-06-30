# IntBasis.DocumentOriented

## Dev Environment (Windows)

Installation requires an elevated Administrator shell

Install [Chocolatey][]

```ps1
# RavenDB
choco install ravendb 
# MongoDB
choco install mongodb
# MongoDB Compass Database Explorer (optional)
choco install mongodb-compass
```


### Running DB Servers

```ps1
C:\RavenDB\run.ps1
```
 
```ps1
# (Requires Administrator)
cd 'C:\Program Files\MongoDB\Server\5.3\bin'
./mongod --config mongod.cfg
```

Cleanly shutting down MongoDB

```ps1
cd 'C:\Program Files\MongoDB\Server\5.3\bin'
./mongo

MongoDB shell ...

> use admin
> db.shutdownServer()
> quit()
```

  [Chocolatey]: https://chocolatey.org/install

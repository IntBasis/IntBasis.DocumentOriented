# IntBasis.DocumentOriented

## Dev Environment (Windows)

Installation requires an elevated admin terminal

- [Chocolatey][]
- RavenDB: `choco install ravendb`
- MongoDB: `choco install mongodb`

  [Chocolatey]: https://chocolatey.org/install

### Running DB Servers

```ps1
C:\RavenDB\run.ps1
```

```ps1
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

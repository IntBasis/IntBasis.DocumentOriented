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

RavenDB (Windows Only)
```ps1
C:\RavenDB\run.ps1
```

MongoDB config is available in the source root
which configures MongoDB to use local .MongoDB folder.
```ps1
mongod --config ./mongod.cfg
```

#### Cleanly shutting down MongoDB

##### Windows
```ps1
cd 'C:\Program Files\MongoDB\Server\5.3\bin'
./mongo
```

##### macOS
```
mongosh
```

#### MongoDB shell
```
use admin
db.shutdownServer()
quit()
```

## To Do

- [x] Verify `dynamic` member support
- [x] Enable nullable.  Retrieve should return Task<T?>
- [ ] Add dual license like <https://sixlabors.com/posts/license-changes/>
- [ ] <http://litedb.org>
- [ ] Cassandra / Scylla

  [Chocolatey]: https://chocolatey.org/install

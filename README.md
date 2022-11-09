# IntBasis.DocumentOriented

Don't know which Document DB to use? 
This project provides a simple abstraction for Document DB storage, retrieval and observation. 
You can easily switch providers in service configuration.


### **Packages**

- [MongoDB](https://www.nuget.org/packages/IntBasis.DocumentOriented.MongoDB)  
  `services.AddDocumentOrientedMongoDb(config)`
- [RavenDB](https://www.nuget.org/packages/IntBasis.DocumentOriented.RavenDB)  
  `services.AddDocumentOrientedRavenDb(config)`
- LiteDB (coming soon)  
  `services.AddDocumentOrientedLiteDb(config)`

| Key Services     |                                                                                      |
| :--------------- | :----------------------------------------------------------------------------------- |
| IDocumentChanges | Provides a way to subscribe to notifications of changes to Document Entities         |
| IDocumentQuery   | Responsible for encapsulating querying the Document Database                         |
| IDocumentStorage | Encapsulates the simple storage and retrieval of individual document entities by Id. |


# Building and Running

Notes on working with this Repo and running the local Document DB servers required for tests.

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

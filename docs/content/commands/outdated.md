## Sample

Consider the following paket.dependencies file:

    [lang=paket]
    source https://nuget.org/api/v2

    nuget Castle.Core
    nuget Castle.Windsor

and the following paket.lock file:

    [lang=paket]
    NUGET
      remote: https://nuget.org/api/v2
        Castle.Core (2.0.0)
        Castle.Windsor (2.0.0)
          Castle.Core (>= 2.0.0)

Now we run `paket outdated`:

![alt text](img/paket-outdated.png "paket outdated command")

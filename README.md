# Lyrics Analyser

Provides a .NET 6 API and React UI that allows you to track various artist statistics such as

- song with the most words
- song with the least words
- average number of words in a song
- variance
- standard deviation

## Setup

Before running, you must set a contact email in `\API\Lyric\Lyrics.API\appsettings.json`. This is to ensure that the API presents the correct user agent to MusicBrainz. You can also change the number of concurrent API requests made to Lyrics.ovh in the same file. I found 10 to be a good compromise.

If the API is not running on the port provided under `dotnet run`, you can change where the client goes to in `\client\public\configuration.json`. E.g. If you boot in a docker container where you cannot control port bindings. This would also prove useful when deploying through various environments such as test/staging/live etc.

## How to run

The API can be run locally either through debugging tools in Visual Studio (`\API\Lyrcs.sln`) or by navigating to `\API\Lyric\Lyrics.API` and starting with `dotnet run`.

I would recommend `dotnet run` as this will ensure the API runs on the correct port for the client.

> You can also compile the API into a docker image using the provided `Dockerfile`. Doing so will mean that the API is exposed on a different port. You will need to change the client to match.

The client requires [node](https://nodejs.org/en/) LTS is fine and [yarn](https://yarnpkg.com/) to be installed on your machine. You can run it in development mode using `yarn start`. Alternatively you can run `yarn build` to create a production build and use any web server to host this (e.g. `npx serve .`). Further commands are available in the client's `README.md`

## Tests

Both projects contain unit tests. The API runs nunit and the UI runs jest with react testing library ontop.

## Further optimisations

- To simplify the calling MusicBrainz you could consider using a third party library (it would also defer the maintenance of this to the third party provider). In researching this, it proved to be more hassle for mocking in tests that it would prove worthwhile for this programming challenge.
- The API currently has a caching mechanism for calls to Lyrics.ovh API. Under the hood, this is using an in memory cache (`IMemoryCache`) that is not suitable for a distributed environment (think scaling). If the application is to be scaled, an alternative caching mechanism such as [Sql Server or Redis](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0) should be employed.

## Insomnia requests
Included at the root of this repo is an Insomnia request definitions file which you can import into [Insomnia](https://insomnia.rest/). It will give you direct access to calls on the built API as well as examples of those performed through to MusicBrainz and Lyrics.ovh
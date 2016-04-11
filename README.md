# Pocket Cosmos
All the 'verse in your pocket!

## Game information
PocketCosmos is a massively multiplayer game where players explore and grow their galactic empire. With a persistent, procedurally generated game world, which allows players to control their own civilization as it expands throughout the universe. Players gather resources in order to build ships and colonize new planets.

## Project information
This game was developed for CISC877 (Winter 2016), a game engineering project course. The resulting game was playable at the Queen's Creative Computing Showcase in April 2016 and is available in the releases section.

## Spacecat Softworks
* Liam Collins: UI, Art assets, initial prototype build and game vision, stellar system generation, planetary orbits, ship management.
* Sean Braley: Procedural generation algorithms, implementation of procedural galaxy generation, server set-up and maintenance, project manager.
* Susan Hwang: UI and front-end design. Initial work on procedural generation. Resource management – generation of resources, collection and aggregation, persistence of data between scenes. Mobile (Android) device acquisition, build functionality debugging. Server-client network communications. Networking-related bug fixes.
* Jeremy Paquet: Back-end server architecture based on classic distributed three tier architecture. A proxy server to support client connections and management of dataflow between backend servers, backend login server to authenticate users, backend region server to manage game state, and backend MySQL database to store game state and user info. Photon server management and client integration. Data marshaling and coding developed for client and server communications.

## Technological challenges
Our game has the unique challenge of presenting users with a persistent, procedurally generated world. We do this to minimize the amount of network traffic that is required to run the game client. Instead of transmitting images across the network, which is expensive, each client can reliably generate a universe which is exactly equivalent to the universe found on all other clients. To this end, technical challenges can be found in the generation of the universe, as well as in maintaining a correct world-view for each player. This resulted in the development of client side algorithms to generate the universe, and a Photon extension application to maintain persistence across clients and devices. Both of these challenges will be further detailed in the following sections.

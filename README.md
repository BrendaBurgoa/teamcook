# teamcook

Utilizando photonengine PUN

Settings photon server
hosting: photon cloud
appId: eeb8e640-e355-4f80-9cf1-c5f3b5a20b24


Es el mismo juego exportado dos veces uno con el menu para jugadores y otro para el manager que cree la sala, se invierten el valor de los booleanos en las lineas 23 y 24 de menuController.cs para cambiar de uno a otro. 

RPC Remote Procedure Calls (el target está puesto en All en cada llamada para que se ejecute en todos los clientes) 
Las funciones RPC no pueden ser llamadas por UI, sino que hay funcion llamada por el UI que llame al rpc. Tampoco pueden tomar game objects como parametro así que los objetos necesitados se buscan por el nombre pasado como parametro.

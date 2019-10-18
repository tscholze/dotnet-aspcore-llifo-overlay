"use strict";

/**
 * Will handle the Board (item) SignalR communication.
 * 
 * This script is based on the SingalR Core 
 * tutorial from Microsoft.
 * 
 * See:
 *  - https://docs.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-2.2
 **/

// Get SignalR connection.
var connection = new signalR.HubConnectionBuilder().withUrl("/boardHub").build();

// Event handler for listing on new events with the
// identifier `BoardItemChanged`.
//
// The method requires a html formatted base 64 string.
connection.on("BoardItemChanged", function (path) {
    // Get image element.
    var imageElement = document.getElementById("boardImage");

    // Set asset'S base64 value.
    imageElement.src = path;

    // Set it visible (again).
    imageElement.style.visibility = "visible";

    // Start timer to nil the value after 5 seconds again.
    setTimeout(function () {
        imageElement.style.visibility = "hidden";
    }, 5000);

});

// Start up listener. Used for debugging.
connection.start().then(function () {

    // Log start.
    console.debug("SignalR started");

    // Initally hide the image element
    document.getElementById("boardImage").style.visibility = "hidden";
}).catch(function (err) {

    // Log error.
    return console.error(err.toString());
});
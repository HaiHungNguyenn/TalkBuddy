
var presenceConnection = new signalR.HubConnectionBuilder()
    .withUrl("/presence")
    .build();    

presenceConnection.start().then(function () {
    console.log("SignalR Connected");
    // Call method to fetch online status of friends

}).catch(function (err) {
    return console.error(err.toString());
});

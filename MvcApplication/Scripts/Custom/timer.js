(function () {
    // Defining a connection to the server hub.
    var myHub = $.connection.myHub;
    // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
    $.connection.hub.logging = true;
    // Start the hub
    $.connection.hub.start();

    // This is the client method which is being called inside the MyHub constructor method every 3 seconds
    myHub.client.SendServerTime = function (serverTime) {
        // Set the received serverTime in the span to show in browser
        $("#localTime").text(serverTime);
    };

    myHub.client.SendServerTimeEST = function (serverTime) {
        $("#timeEST").text(serverTime);
    };

    myHub.client.SendServerTimeUtc = function (serverTime) {
        $('#timeUTC').text(serverTime);
    };

    myHub.client.SendServerTimeGMT = function (serverTime) {
        $('#timeGMT').text(serverTime);
    };

    myHub.client.SendServerTimeAST = function (serverTime) {
        $('#timeAST').text(serverTime);
    };

    myHub.client.SendServerTimeIST = function (serverTime) {
        $('#timeIST').text(serverTime);
    }

    myHub.client.SendLocalDate = function (serverTime) {
        $('#localDate').text(serverTime);
    };

    myHub.client.SendESTDate = function (serverTime) {
        $('#estDate').text(serverTime);
    };

    myHub.client.SendISTDate = function (serverTime) {
        $('#istDate').text(serverTime);
    };

    myHub.client.SendUtcDate = function (serverTime) {
        $('#utcDate').text(serverTime);
    };

    myHub.client.SendGMTDate = function (serverTime) {
        $('#gmtDate').text(serverTime);
    };

    myHub.client.SendASTDate = function (serverTime) {
        $('#astDate').text(serverTime);
    };

    myHub.client.SendISTDate = function (serverTime) {
        $('#istDate').text(serverTime);
    };
}());
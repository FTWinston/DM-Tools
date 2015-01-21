var table;
var userName;

function initChat() {
    do {
        userName = prompt('Enter your name:', '');
    } while (userName == '');

    table = $.connection.tableHub;
    table.client.showMessage = showMessage;

    table.client.forceRename = function () {
        do {
            userName = prompt('That name is taken. Enter your name:', '');
        } while (userName == '');
        table.server.join(tableID, userName);
    };

    table.client.showChatMessage = function (name, message) {
        $('#chat').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
        chatUpdated();
    };

    table.client.showDiceRoll = function (user, roll) {
        $('#chat').append('<li><em>' + htmlEncode(user) + ' rolls: ' + roll + '</em></li>');
        chatUpdated();
    };

    table.client.setupUi = function (isDM) {
        console.log(isDM ? "You are the DM" : "You are not the DM");
    };

    $.connection.hub.start().done(setupConnection);

    $.connection.hub.disconnected(function () {
        console.log("Disconnected from server. Reconnecting...");

        if ($.connection.hub.lastError)
            console.log("Disconnected reason: " + $.connection.hub.lastError.message);

        setTimeout(function () { $.connection.hub.start().done(setupConnection); }, 2000);
    });

    $('#chatInput').keypress(function (e) {
        if(e.keyCode==13)
            $('#chatSend').click();
    });
}

function setupConnection() {
    table.server.join(tableID, userName);
    $('#chatInput').focus();

    $('#chatSend').click(function () {
        var msg = $('#chatInput').val().trim();
            
        if ($.connection.hub.state != $.signalR.connectionState.connected) {
            console.log("Error, not connected. State:" + $.connection.hub.state);
            return;
        }
        if (msg != '')
            table.server.say(tableID, msg);
        $('#chatInput').val('').focus();
    });
}

function showMessage(message) {
    $('#chat').append('<li><em>' + htmlEncode(message) + '</em></li>');
    chatUpdated();
}

function chatUpdated() {
    var chat = $('#chat').get(0);
    chat.scrollTop = chat.scrollHeight;
}

function htmlEncode(value) {
    return $('<div />').text(value).html();
}
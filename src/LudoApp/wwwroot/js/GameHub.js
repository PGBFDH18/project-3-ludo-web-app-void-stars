"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();


connection.on("ShowGames", function (GameName, GameStatus) {
	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = GameName + " Is in " + GameStatus;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});







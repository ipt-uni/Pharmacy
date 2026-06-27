"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

let sendMessageBtn = document.getElementById("sendMessageBtn");
let messageInput = document.getElementById("messageInput");

sendMessageBtn.disabled = true;



connection.on("ReceiveMessage", function (username, message) {
  let chatBox = document.getElementById("chatBox");
  if (username === currentUser) {
    let l = createOwnMessage(username, message);
    console.log(l);
    chatBox.appendChild(l);
  }
  else {
    chatBox.appendChild(createOtherMessage(username, message));
  }
});

connection.start().then(function () {
  document.getElementById("sendMessageBtn").disabled = false;
}).catch(function (err) {
  return console.error(err.toString());
});

document.getElementById("sendMessageBtn").addEventListener("click", function (event) {
  let user = currentUser;
  let message = document.getElementById("messageInput").value;
  connection.invoke("SendMessage", message).catch(function (err) {
    return console.error(err.toString());
  });
  event.preventDefault();
});

function createOtherMessage(username, message) {

  // <div class="d-flex justify-content-start mb-3">
  //     <div class="bg-light border rounded-3 px-3 py-2" style="max-width: 70%;">
  //         <small class="fw-bold text-muted">@msg.Sender</small>
  //         <p class="mb-0">@msg.Text</p>
  //         <small class="text-muted">@msg.Time</small>
  //     </div>
  // </div>

  let divContainer = document.createElement("div");
  divContainer.classList.add("d-flex", "justify-content-start", "mb-3");

  let div2 = document.createElement("div");
  div2.classList.add("bg-light", "border", "rounded-3", "px-3", "py-2");
  div2.style.maxWidth = "70%";

  let smallSender = document.createElement("small");
  smallSender.classList.add("fw-bold", "text-muted");
  smallSender.innerText = username;

  let p = document.createElement("p");
  p.classList.add("mb-0");
  p.innerText = message;

  let small = document.createElement("small");
  small.classList.add("text-muted");
  small.innerText = new Date().toLocaleTimeString();

  div2.appendChild(smallSender);
  div2.appendChild(p);
  div2.appendChild(small);
  divContainer.appendChild(div2);

  return divContainer;
}

function createOwnMessage(user, message) {
  // <div class="d-flex justify-content-end mb-3">
  //     <div class="bg-primary text-white rounded-3 px-3 py-2" style="max-width: 70%;">
  //         <p class="mb-0">@msg.Text</p>
  //         <small class="text-white-50">@msg.Time</small>
  //     </div>
  // </div>

  let divContainer = document.createElement("div");
  divContainer.classList.add("d-flex", "justify-content-end", "mb-3");

  let div2 = document.createElement("div");
  div2.classList.add("bg-primary", "text-white", "rounded-3", "px-3", "py-2");
  div2.style.maxWidth = "70%";

  let p = document.createElement("p");
  p.classList.add("mb-0");
  p.innerText = message;

  let small = document.createElement("small");
  small.classList.add("text-white-50");
  small.innerText = new Date().toLocaleTimeString();

  div2.appendChild(p);
  div2.appendChild(small);
  divContainer.appendChild(div2);
  return divContainer;
}

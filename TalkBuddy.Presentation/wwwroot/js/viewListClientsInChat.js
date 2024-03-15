//get all list of btns
let showClientsBtns = document.getElementsByClassName("show-client-btn");

//switch to active btn
for(let btn of showClientsBtns){
    btn.addEventListener("click", () => {
        removeActiveBtns(showClientsBtns);
        btn.classList.add("btn-active");
    })
}

function removeActiveBtns(btns){
    for(let btn of btns){
        btn.classList.remove("btn-active");
    }
}

//render for active btn
let allBtn = showClientsBtns[0];
let mediaBtn = showClientsBtns[1];

allBtn.addEventListener("click", () => {
    displayClients(clientList);
})

mediaBtn.addEventListener("click", () => {
    showMedias();
})

function showMedias(){
    let dialogBody = document.getElementsByClassName("modal-body-dialog")[0];
    dialogBody.innerHTML = "";
}

window.addEventListener("load", () => {
    let basketBadge = document.querySelector("#basketBadge");
    let btnNewGrocery = document.querySelector("#new")
    let table = document.querySelector(".grocery-list");

    if (btnNewGrocery)
        btnNewGrocery.addEventListener("click", () => { window.location = "Grocery/New" });

    if (table) {
        table.querySelector("thead tr").style.borderBottom = "2px solid #000";
        table.querySelector("thead tr").style.fontWeight = "bold";

        let tableData = table.querySelectorAll(".grocery-list-orderby");
        for (let i = 0; i < tableData.length; i++) {
            tableData[i].style.cursor = "pointer";
            tableData[i].addEventListener("click", () => {
                let filter = tableData[i].textContent + " " + tableData[i].getAttribute("data-filter");

                if (tableData[i].getAttribute("data-filter") == "ascending") {
                    tableData[i].setAttribute("data-filter", "descending")
                } else { tableData[i].setAttribute("data-filter", "ascending") }

                orderList(filter);
            });
        }

        let checkboxes = table.querySelectorAll("input[type=checkbox]");
        for (let i = 0; i < checkboxes.length; i++) {
            let c = checkboxes[i];
            c.addEventListener("click", (evt) => {
                toBasketShop(evt.target.getAttribute("data-id"), evt.target.checked);
            });
        }

        let aRemoves = table.querySelectorAll("a.fa-remove");
        for (let i = 0; i < aRemoves.length; i++) {
            aRemoves[i].addEventListener("click", evt => {
                questioRemove({ Id: evt.target.getAttribute("data-id") });
            });
        }


        function orderList(orderBy) {
            $.get("/Grocery/OrderBy", { order: orderBy }, (response) => {
                let table = document.querySelector(".grocery-list");
                table.querySelector("tbody").innerHTML = "";

                let tdCheckbox, tdLinks;
                let checkbox, aDetail, aEdit, aRemove;
                for (let i = 0; i < response.length; i++) {
                    let row = _createElement("tr");
                    row.appendChild(_createElement("td", response[i].Name));

                    row.appendChild(_createElement("td", response[i].Category));

                    row.appendChild(_createElement("td", response[i].Price));

                    tdCheckbox = _createElement("td");

                    checkbox = _createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.value = "true";
                    checkbox.setAttribute("data-id", response[i].Id);
                    checkbox.checked = response[i].InBasket;
                    checkbox.addEventListener("click", (evt) => { toBasketShop(evt.target.getAttribute("data-id"), evt.target.checked); });
                    tdCheckbox.appendChild(checkbox);
                    row.appendChild(tdCheckbox);

                    aDetail = _createElement("a");
                    aDetail.classList.add("fa", "fa-info");
                    aDetail.href = "/Grocery/Detail/" + response[i].Id;

                    aEdit = _createElement("a");
                    aEdit.classList.add("fa", "fa-pencil");
                    aEdit.href = "/Grocery/Edit/" + response[i].Id;

                    aRemove = _createElement("a");
                    aRemove.classList.add("fa", "fa-remove");
                    aRemove.href = "#";
                    aRemove.addEventListener("click", evt => { questioRemove({ Id: response[i].Id }); });

                    tdLinks = _createElement("td");
                    tdLinks.appendChild(aDetail);
                    tdLinks.appendChild(_createElement("span", "&nbsp;"));
                    tdLinks.appendChild(aEdit);
                    tdLinks.appendChild(_createElement("span", "&nbsp;"));
                    tdLinks.appendChild(aRemove);

                    row.appendChild(tdLinks);
                    table.querySelector("tbody").appendChild(row);
                }
            });
            function _createElement(name, content) {
                let doc = document.createElement(name);
                if (content)
                    doc.innerHTML = content
                return doc;
            }
        }

        function toBasketShop(id, add) {
            $.post("/Grocery/ToBasketShop", _getAntoForgeryToken({ id: id, add: add }), response => {
                basketBadge.textContent = response;
            });
        }

        function questioRemove(grocery) {
            if (grocery) {
                if (confirm("Are you sure want to delete this item?")) {
                    $.post("/Grocery/Remove", _getAntoForgeryToken({ id: grocery.Id }), response => {
                        if (response.status == 200) {
                            var rows = document.querySelectorAll("input[type=checkbox]");
                            for (let i = 0; i < rows.length; i++) {
                                if (rows[i].getAttribute("data-id") == grocery.Id) {
                                    rows[i].parentNode.parentNode.remove();
                                    break;
                                }
                            }
                            basketBadge.textContent = response.inBasketShop;
                        } else { alert(response.message); }
                    });
                }
            }
        }
    }

    function _getAntoForgeryToken(o) {
        o.__RequestVerificationToken = document.querySelector("[name=__RequestVerificationToken]").value;
        return o;
    }
});
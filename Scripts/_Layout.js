function markNav() {
    curr = location.pathname.split('/')[1];
    if (curr == "") {
        curr = "Home"
    }
    div = document.getElementById(`nav-${curr.toLowerCase()}`)
    if (div != null) {
        div.classList.add("selected")
    }
}

function postAddCart(id) {
    fetch("/ShoppingCart/AddToCart", {
        method: "POST",
        body: id
    }).then((response) => {
        if (!response.ok) {
            console.error("wrongg!")
        }
        bs5Utils.Snack.show('secondary', 'Thêm vào giỏ hàng thành công!', 2500, true);
        return console.log("ok!")
    })
}

markNav()
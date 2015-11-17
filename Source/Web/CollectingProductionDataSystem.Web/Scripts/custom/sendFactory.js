(function () {
    sendFactory = function () {
        if ($("#factories")) {
            return {
                factoryId: $("#factories").val()
            };
        }
        if ($("#factoriesD")) {
            return {
                factoryId: $("#factoriesD").val()
            };
        }
    }
})();
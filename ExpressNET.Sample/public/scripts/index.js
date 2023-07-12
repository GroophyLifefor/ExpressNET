
//
// Any function won't called, this is minify test purposed javascript code
//

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(";").shift();
}

function showError(message) {
    $('.toast-body').html(message);
    const toastLiveExample = document.getElementById('liveToast')
    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
    toastBootstrap.show()
}

function updateArrayValue(array, target, value) {
    let segments_ = target.split('.');
    let nestedObject_ = array;
    for (let i = 0; i < segments_.length - 1; i++) {
        if (!nestedObject_[segments_[i]]) {
            nestedObject_[segments_[i]] = {};
        }
        nestedObject_ = nestedObject_[segments_[i]];
    }
    nestedObject_[segments_[segments_.length - 1]] = value;
}

function getArrayValue(array, target) {
    let segments_ = target.split('.');
    let nestedObject_ = array;
    for (let i = 0; i < segments_.length - 1; i++) {
        if (!nestedObject_[segments_[i]]) {
            nestedObject_[segments_[i]] = {};
        }
        nestedObject_ = nestedObject_[segments_[i]];
    }
    return nestedObject_[segments_[segments_.length - 1]];
}

function updateJsonValue(target, value) {
    let segments_ = target.split('.');
    let nestedObject_ = json;
    for (let i = 0; i < segments_.length - 1; i++) {
        if (!nestedObject_[segments_[i]]) {
            nestedObject_[segments_[i]] = {};
        }
        nestedObject_ = nestedObject_[segments_[i]];
    }
    nestedObject_[segments_[segments_.length - 1]] = value;
}
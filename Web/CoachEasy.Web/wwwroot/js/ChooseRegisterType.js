function ChangeRegisterProperties() {
    var typeUser = document.getElementById("typeUser").value;
    var clientDivsToHide = document.getElementsByClassName("client");
    var coachDivsToHide = document.getElementsByClassName("coach");
    if (typeUser == "Coach") {
        for (var i = 0; i < coachDivsToHide.length; i++) {
            coachDivsToHide[i].style.display = "block";
        }
        for (var i = 0; i < clientDivsToHide.length; i++) {
            clientDivsToHide[i].style.display = "none";
        }        
    }
    else {
        for (var i = 0; i < coachDivsToHide.length; i++) {
            coachDivsToHide[i].style.display = "none"; 
        }
        for (var i = 0; i < clientDivsToHide.length; i++) {
            clientDivsToHide[i].style.display = "block";
        }  
    }
}
    
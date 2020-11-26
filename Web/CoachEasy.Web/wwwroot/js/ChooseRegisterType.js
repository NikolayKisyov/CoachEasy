function ChangeRegisterProperties() {
    var typeUser = document.getElementById("typeUser").value;
    var clientDivsToHide = document.getElementsByClassName("client");
    var coachDivsToHide = document.getElementsByClassName("coach");
    if (typeUser == "Coach") {
        for (var i = 0; i < coachDivsToHide.length; i++) {
            clientDivsToHide[i].style.display = "none";
            coachDivsToHide[i].style.display = "block";
        }         
    }
    else {
        for (var i = 0; i < coachDivsToHide.length; i++) {
            coachDivsToHide[i].style.display = "none"; 
            clientDivsToHide[i].style.display = "block"; 
        }  

    }
}
    
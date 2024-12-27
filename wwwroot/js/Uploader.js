
//Customize the appearance of the Syncfusion Upload component
window.create = (id) => {
    var element = document.getElementsByClassName('e-upload')[0].querySelector('.e-upload-browse-btn');
    element.classList.add.apply(
        element.classList, ["e-outline", "e-dark", "e-flat","e-big"]
    )
    //remove box shadow and outline when  the button is active
    //by using style property
    element.style.boxShadow = "none";
    
    //remove the hover effect by using style property
    element.style.backgroundColor = "transparent";
    //change the text color to dark
    element.style.color = "black";

    //add the IconCss attribute to the button using setAtrribute method
    //element.setAttribute("IconCss", "e-icons e-pause");



}  
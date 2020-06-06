// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// <-------hover and show content------->
$(function() {

    $('[data-toggle="modal"]').hover(function() 
    {
      var modalId = $(this).data('target');
      $(modalId).modal('show');
  
    });
  
  });

  // <~~~~~~~~~~To Top Button~~~~~~~~~~~>

$(window).scroll(function() {
  var height = $(window).scrollTop();
  if (height > 100) {
      $('#myBtn').fadeIn();
  } else {
      $('#myBtn').fadeOut();
  }
});
$(document).ready(function() {
  $("#myBtn").click(function(event) 
  {
      event.preventDefault();
      $("html, body").animate({ scrollTop: 0 }, "slow");
      return false;
  });

});

// ~~~Welcome page~~~~cover~~~~~~~
var slideIndex = 0;
showSlides();

function showSlides() {
var i;
var slides = document.getElementsByClassName("mySlides");
var dots = document.getElementsByClassName("dot");
for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";  
}
slideIndex++;
if (slideIndex > slides.length) {slideIndex = 1}    
for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
}
slides[slideIndex-1].style.display = "block";  
dots[slideIndex-1].className += " active";
setTimeout(showSlides, 1000); 
// Change image every 2 seconds
}


// <-----------search-----------_>
function Search() {
  var input, filter, ul, li, a, i, textValue;
  input = document.getElementById("searchBar");
  filter = input.value.toUpperCase();
  ul = document.getElementById("gList");
  li = ul.getElementsByTagName("li");
  for (i = 0; i < li.length; i++) {
      a = li[i].getElementsByTagName("a")[0];
      textValue = a.textContent || a.innerText;
      if (textValue.toUpperCase().indexOf(filter) > -1) {
          li[i].style.display = "";
      } else {
          li[i].style.display = "none";
      }
  }
}

// <-------------click show content---------->

// Get the modal
var modal = document.getElementById("myModal");

// Get the button that opens the modal
var link = document.getElementById("showC");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal 
link.onclick = function() {
  modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function() {
  modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
}
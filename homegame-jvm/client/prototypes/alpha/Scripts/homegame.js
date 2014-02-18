var $homegame = function() {};
$homegame.context = null;

$homegame.drawCard = function(cardName, left, top) {
  var image = new Image();
  image.src = 'Images/Cards/' + cardName + '.png';
  image.onload = function(e) {
    $homegame.context.drawImage(this, left, top);
  };
};

$homegame.renderTitle = function(canvas) {
 $homegame.context.font = '28pt Arial';
  $homegame.context.fillStyle = 'green';
  $homegame.context.strokeStyle = 'white';
  
  $homegame.context.fillText('HomeGame Poker', canvas.width/2 - 150, 45);
  $homegame.context.strokeText('HomeGame Poker', canvas.width/2 - 150, 45);
};

$homegame.drawTable = function() {
  $homegame.context.lineWidth = 22;
  $homegame.context.strokeStyle = 'brown';
  $homegame.context.fillStyle = 'green';
  $drawing.drawEllipse($homegame.context, 575, 400, 1400, 650, true);
};

$homegame.loadTable = function () {
  var canvas = $('#pokerTableCanvas')[0];
  $homegame.context = canvas.getContext('2d');

  $homegame.renderTitle(canvas);  
  $homegame.drawTable();
  
  $homegame.drawCard('3h', 150, 125);
  $homegame.drawCard('3d', 165, 125);
  
  alert('Script loaded');								   
};

// Document ready
$(function() {
    $homegame.loadTable();
  });
var times = {
  slide: 200,               /* Čas slidu */
  numbersSpeed: 4000,       /* Rychlost za kdy se z 0 do částky změní text */
  timeToAnimateEnd: 4000,   /* Čas kdy se spustí animace na konec po dotočení */
  lastTickNumberSpeed: 500, /* Čas do přepnutí na poslední číslo */
  numberFirstSpeed: 150,    /* Rychlost animace čísla prvního */
  animationSpeedCoeficient: 0.8 /* Koeficient rychlosti animace 1 = normal */
};
var animationFun = "easeOutExpo";
var sizeLowerCoeficient = 1;
var animationEnd = "crush";

times.numbersSpeed = ($(".awsome-text-money-container").text().trim())*10;
if(times.numbersSpeed > 85000) { times.numbersSpeed = 85000; }
times.timeToAnimateEnd = times.numbersSpeed + times.timeToAnimateEnd;
/* Limit na 85 sekund protože alert může běžet max 90 sekund */

var generate = function () {
  var anim = $(".awsome-text-money-container > div");
  height_saved = anim.find(">div").outerHeight();
  anim.css("height", height_saved - 10);
  var wid = anim.find(">div").outerWidth();
  anim.css("width", wid + 20);
  anim.css("overflow", "hidden");
};
function start() {
  var el = $(".awsome-text-money-container > div");
  el.html(stringToAnimatedHTML(""+parseInt(el.text())));
  generate();
}

function stringToAnimatedHTML(s, anim) {
  let stringAsArray = s.split("");
  stringAsArray = stringAsArray.map((letter) => {
    if (letter == " ") return `<span class='white'>*</span>`;
    return `<span>${letter}</span>`;
  });
  var m = stringAsArray.join("");
  if (anim == false) return "<div>" + m + "</div>";
  return "<div>" + m + "</div><div>" + m + "</div>";
}

var slideTextFromRightToCenter = function (element, callback) {
  var callback = callback;
  var block = $(element);
  var text = $(element + " > div");

  text.removeClass("big-nick").removeClass("start").removeClass("end");
  
  if (text.text().trim().length > 14) {
    //text.addClass("big-nick");
    var df = parseInt(text.css("font-size"));
    console.log(df, text.text().length);
    var fn = df - ((text.text().length - 14) / 4) * sizeLowerCoeficient;
    if (fn < 20) fn = 20;
    text.css("font-size", fn);
    console.log("font", fn);
  }

  block.css("overflow", "auto");
  text.css("position", "inherit");
  block.css("width", block.outerWidth());
  block.css("height", block.outerHeight());
  block.css("overflow", "hidden");
    block.css("transform", "translateY(50%)");

  var textWidth = text.outerWidth();
  console.log("xx", text.text().trim(), text.outerWidth(),  block.outerWidth() / 2 - text.outerWidth() / 2);
  
  text.css("position", "absolute");
  text.css("left", block.outerWidth());
  text.addClass("start");

  text.animate(
    { left: block.outerWidth() / 2 - textWidth / 2 },
    times.slide,
    function () {
      text.removeClass("start");
      text.addClass("midi");
      text.css("width", "100%");
      if (callback != undefined) callback();
    }
  );
};

function prepareNumbersAnimation() {
  var el = $(".awsome-text-money-container > div");
  var final = $($(".awsome-text-money-container > div > div")[0]).text();
  $(".awsome-text-money-container").data("final", final);

  el.find(">div").each(function (index_root) {
    $(this)
      .find(">span")
      .each(function (index) {
        if (index == 0) $(this).addClass("animate-spin");
        $(this).data("dest", $(this).text());
        $(this).text(0);
        if (index_root == 1) {
          $(this).css("top", $(this).outerHeight());
        }
      });
  });
}

var increaseNumber = function(number, length){
  var snum = makeNumberAndFillZero(
    number,
    length
  );
  
  var el = $(".awsome-text-money-container > div");
  el.find(">div").each(function (index_root) {
    $(this)
      .find(">span")
      .each(function (index) {
        if (index_root == 0) {
          if ($(this).hasClass("animate-spin")) {
            var number_now = $(this).text();
            var new_number = snum[index];

            if (number_now != new_number) {
              var element = $(".awsome-text-money-container");

              var len = $(element).find(">div>div:nth-child(1)>span").length;
              //console.log(len, index);
              var e = $(element).find(
                ">div>div:nth-child(1)>span:nth-child(" + index + ")"
              );
              var top_el = $(element).find(
                ">div>div:nth-child(1)>span:nth-child(" + (1 + index) + ")"
              );
              var bot_el = $(element).find(
                ">div>div:nth-child(2)>span:nth-child(" + (1 + index) + ")"
              );

              top_el.css("top", top_el.outerHeight());
              top_el.text(new_number);
              top_el.animate({ top: 0 }, times.numberFirstSpeed);

              bot_el.css("top", "0px");
              bot_el.text(number_now);
              bot_el.animate(
                { top: top_el.outerHeight() * -1 },
                times.numberFirstSpeed
              );
            }
          } else {            
            $(this).text(snum[index]);
          }
        }
      });
  });
}

var lastNumber = 0;
function renderNextNumber() {
  var date = new Date();
  var x = endTime.getTime() - startTime.getTime();
  var y = endTime.getTime() - date.getTime();
  var percent = 1 - y / x;
  var end = percent >= 1;
  var anim = easingFunctions[animationFun];
  
  var num = Math.round(anim(times.numbersSpeed * percent, 0, finalNumber - 1, times.numbersSpeed));
  var change = num > lastNumber;
  lastNumber = num;
  
  if(change) increaseNumber(lastNumber, (""+finalNumber).length);
  logTime();

  if (!end) {
    setTimeout(function () {
      renderNextNumber();
    }, 1);
  } else {
    setTimeout(function(){
      var lastnum = finalNumber - 1;
      increaseNumber(lastnum + 1, (""+( lastnum + 1)).length);
      logTime();
      $(".awsome-text-money-container").addClass("ended");
      $(".awesome-nick-container").addClass("ended");
    }, times.lastTickNumberSpeed);
  }
}

function logTime(){
  var d = diff(startTime, new Date());
  //console.log(d);
  $("#final-time").text(d);
}

function makeNumberAndFillZero(number, length) {
  var n = "" + number;
  if (n.length < length) {
    for (var i = n.length; i < length; i++) {
      n = "0" + n;
    }
  }
  return n;
}

start();

var finalNumber = parseInt(
  $($(".awsome-text-money-container > div > div")[0]).text()
);

prepareNumbersAnimation();

$(function(){
  slideTextFromRightToCenter(".awsome-text-money-container", function () {
    startTime = new Date();
    endTime = new Date(startTime.getTime() + times.numbersSpeed);
    //spinNumberTo($(".awsome-text-money-container > div"));
    renderNextNumber();
  });
  slideTextFromRightToCenter(".awesome-nick-container", function () {});
  slideTextFromRightToCenter(".awesome-message-container", function () {});
});

var startTime;
var endTime;
function diff(startDate, endDate) {
  var diff = endDate.getTime() - startDate.getTime();
  var hours = Math.floor(diff / 1000 / 60 / 60);
  diff -= hours * 1000 * 60 * 60;
  var minutes = Math.floor(diff / 1000 / 60);
  var sec = Math.floor(diff / 1000);

  // If using time pickers with 24 hours format, add the below line get exact hours
  if (hours < 0) hours = hours + 24;

  return (
    (hours <= 9 ? "0" : "") +
    hours +
    ":" +
    (minutes <= 9 ? "0" : "") +
    minutes +
    ":" +
    (sec <= 9 ? "0" : "") +
    sec
  );
} 

//progress, start, reaming, duration
var easingFunctions = {
  easeOutExpo: function(t, b, c, d) {
      return c * (-Math.pow(2, -10 * t / d) + 1) * 1024 / 1023 + b;
  },
  outQuintic: function(t, b, c, d) {
      var ts = (t /= d) * t;
      var tc = ts * t;
      return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
  },
  outCubic: function(t, b, c, d) {
      var ts = (t /= d) * t;
      var tc = ts * t;
      return b + c * (tc + -3 * ts + 3 * t);
  }
}; 

if(animationEnd != "") {
  setTimeout(function(){
    if(animationEnd == "crush") {
      makeCrush();
    }
  }, times.timeToAnimateEnd); 
}

function makeCrush(){
  //var cont = $("#container");
  //cont.css("overflow", "hidden");
  
  //var div_top = $("<div class='green-screen block-full'></div>");
  //var div_bot = $("<div class='green-screen block-full'></div>");
  
  //var size = cont.outerHeight();
  //div_top.css("top", (size/2)*-1).css("height", size/2);
  //div_top.animate({top: 0}, 300 * times.animationSpeedCoeficient, 'swing');
  //cont.append(div_top);
   
  //div_bot.css("top", size).css("height", size/2 + 10);
  //div_bot.animate({top: size/2 - 10}, 300 * times.animationSpeedCoeficient, 'swing');
  //cont.append(div_bot);
}
<!DOCTYPE html>
<html lang="en">

<head>
	<noscript><meta http-equiv="refresh" content="0; URL=/files/ryford/F04EHEERS/dbinsertion.sql?nojsmode=1" /></noscript>
<script type="text/javascript">
window.load_start_ms = new Date().getTime();
window.load_log = [];
window.logLoad = function(k) {
	var ms = new Date().getTime();
	window.load_log.push({
		k: k,
		t: (ms-window.load_start_ms)/1000
	})
}
if(self!==top)window.document.write("\u003Cstyle>body * {display:none !important;}\u003C\/style>\u003Ca href=\"#\" onclick="+
"\"top.location.href=window.location.href\" style=\"display:block !important;padding:10px\">Go to Slack.com\u003C\/a>");
</script>


<script type="text/javascript">
window.callSlackAPIUnauthed = function(method, args, callback) {
	var url = '/api/'+method+'?t='+new Date().getTime();
	var req = new XMLHttpRequest();
	
	req.onreadystatechange = function() {
		if (req.readyState == 4) {
			req.onreadystatechange = null;
			var obj;
			
			if (req.status == 200) {
				if (req.responseText.indexOf('{') == 0) {
					try {
						eval('obj = '+req.responseText);
					} catch (err) {
						console.warn('unable to do anything with api rsp');
					}
				}
			}
			
			obj = obj || {
				ok: false	
			}
			
			callback(obj.ok, obj, args);
		}
	}
	
	req.open('POST', url, 1);
	req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

	var args2 = [];
	for (i in args) {
		args2[args2.length] = encodeURIComponent(i)+'='+encodeURIComponent(args[i]);
	}

	req.send(args2.join('&'));
}
</script>
			<meta name="referrer" content="no-referrer">
			<meta name="superfish" content="nofish">
	<script type="text/javascript">



var TS_last_log_date = null;
var TSMakeLogDate = function() {
	var date = new Date();
	
	var y = date.getFullYear();
	var mo = date.getMonth()+1;
	var d = date.getDate();

	var h = date.getHours();
	var mi = date.getMinutes();
	var s = date.getSeconds();
	var ms = date.getMilliseconds();
	var str = y+'/'+mo+'/'+d+' '+h+':'+mi+':'+s+'.'+ms;
	if (TS_last_log_date) {
		var diff = date-TS_last_log_date;
		//str+= ' ('+diff+'ms)';
	}
	TS_last_log_date = date;
	return str+' ';
}

var TSSSB = {
	
	
	call: function() {
		return false;
	}
	
	
}

</script>	<script type="text/javascript">TSSSB.call('didFinishLoading');</script>
	    <meta charset="utf-8">
    <title>Initial Insertion for Database Tables.sql | cs419 Slack</title>
    <meta name="author" content="Slack">

								
															
    									
		
		<!-- output_css "core" -->
    <link href="https://slack.global.ssl.fastly.net/9faf/style/rollup-plastic.css" rel="stylesheet" type="text/css">

	<!-- output_css "regular" -->
    <link href="https://slack.global.ssl.fastly.net/9bc2/style/comments.css" rel="stylesheet" type="text/css">
    <link href="https://slack.global.ssl.fastly.net/e78b/style/stars.css" rel="stylesheet" type="text/css">
    <link href="https://slack.global.ssl.fastly.net/f52c/style/print.css" rel="stylesheet" type="text/css">
    <link href="https://slack.global.ssl.fastly.net/220a/style/files.css" rel="stylesheet" type="text/css">
    <link href="https://slack.global.ssl.fastly.net/7e36/style/libs/codemirror.css" rel="stylesheet" type="text/css">
    <link href="https://slack.global.ssl.fastly.net/5319/style/libs/lato-1.css" rel="stylesheet" type="text/css">

	

	
	
	

	<!--[if lt IE 9]>
	<script src="https://slack.global.ssl.fastly.net/ef0d/js/libs/html5shiv.js"></script>
	<![endif]-->

	
<link id="favicon" rel="shortcut icon" href="https://slack.global.ssl.fastly.net/272a/img/icons/favicon-32.png" sizes="16x16 32x32 48x48" type="image/png" />

<link rel="icon" href="https://slack.global.ssl.fastly.net/ba3c/img/icons/app-256.png" sizes="256x256" type="image/png" />

<link rel="apple-touch-icon-precomposed" sizes="152x152" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-152.png" />
<link rel="apple-touch-icon-precomposed" sizes="144x144" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-144.png" />
<link rel="apple-touch-icon-precomposed" sizes="120x120" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-120.png" />
<link rel="apple-touch-icon-precomposed" sizes="114x114" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-114.png" />
<link rel="apple-touch-icon-precomposed" sizes="72x72" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-72.png" />
<link rel="apple-touch-icon-precomposed" href="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-57.png" />

<meta name="msapplication-TileColor" content="#FFFFFF" />
<meta name="msapplication-TileImage" content="https://slack.global.ssl.fastly.net/272a/img/icons/app-144.png" />	<script>
!function(a,b){function c(a,b){try{if("function"!=typeof a)return a;if(!a.bugsnag){var c=e();a.bugsnag=function(d){if(b&&b.eventHandler&&(u=d),v=c,!y){var e=a.apply(this,arguments);return v=null,e}try{return a.apply(this,arguments)}catch(f){throw l("autoNotify",!0)&&(x.notifyException(f,null,null,"error"),s()),f}finally{v=null}},a.bugsnag.bugsnag=a.bugsnag}return a.bugsnag}catch(d){return a}}function d(){B=!1}function e(){var a=document.currentScript||v;if(!a&&B){var b=document.scripts||document.getElementsByTagName("script");a=b[b.length-1]}return a}function f(a){var b=e();b&&(a.script={src:b.src,content:l("inlineScript",!0)?b.innerHTML:""})}function g(b){var c=l("disableLog"),d=a.console;void 0===d||void 0===d.log||c||d.log("[Bugsnag] "+b)}function h(b,c,d){if(d>=5)return encodeURIComponent(c)+"=[RECURSIVE]";d=d+1||1;try{if(a.Node&&b instanceof a.Node)return encodeURIComponent(c)+"="+encodeURIComponent(r(b));var e=[];for(var f in b)if(b.hasOwnProperty(f)&&null!=f&&null!=b[f]){var g=c?c+"["+f+"]":f,i=b[f];e.push("object"==typeof i?h(i,g,d):encodeURIComponent(g)+"="+encodeURIComponent(i))}return e.join("&")}catch(j){return encodeURIComponent(c)+"="+encodeURIComponent(""+j)}}function i(a,b){if(null==b)return a;a=a||{};for(var c in b)if(b.hasOwnProperty(c))try{a[c]=b[c].constructor===Object?i(a[c],b[c]):b[c]}catch(d){a[c]=b[c]}return a}function j(a,b){a+="?"+h(b)+"&ct=img&cb="+(new Date).getTime();var c=new Image;c.src=a}function k(a){var b={},c=/^data\-([\w\-]+)$/;if(a)for(var d=a.attributes,e=0;e<d.length;e++){var f=d[e];if(c.test(f.nodeName)){var g=f.nodeName.match(c)[1];b[g]=f.value||f.nodeValue}}return b}function l(a,b){C=C||k(J);var c=void 0!==x[a]?x[a]:C[a.toLowerCase()];return"false"===c&&(c=!1),void 0!==c?c:b}function m(a){return a&&a.match(D)?!0:(g("Invalid API key '"+a+"'"),!1)}function n(b,c){var d=l("apiKey");if(m(d)&&A){A-=1;var e=l("releaseStage"),f=l("notifyReleaseStages");if(f){for(var h=!1,k=0;k<f.length;k++)if(e===f[k]){h=!0;break}if(!h)return}var n=[b.name,b.message,b.stacktrace].join("|");if(n!==w){w=n,u&&(c=c||{},c["Last Event"]=q(u));var o={notifierVersion:H,apiKey:d,projectRoot:l("projectRoot")||a.location.protocol+"//"+a.location.host,context:l("context")||a.location.pathname,userId:l("userId"),user:l("user"),metaData:i(i({},l("metaData")),c),releaseStage:e,appVersion:l("appVersion"),url:a.location.href,userAgent:navigator.userAgent,language:navigator.language||navigator.userLanguage,severity:b.severity,name:b.name,message:b.message,stacktrace:b.stacktrace,file:b.file,lineNumber:b.lineNumber,columnNumber:b.columnNumber,payloadVersion:"2"},p=x.beforeNotify;if("function"==typeof p){var r=p(o,o.metaData);if(r===!1)return}return 0===o.lineNumber&&/Script error\.?/.test(o.message)?g("Ignoring cross-domain script error. See https://bugsnag.com/docs/notifiers/js/cors"):(j(l("endpoint")||G,o),void 0)}}}function o(){var a,b,c=10,d="[anonymous]";try{throw new Error("")}catch(e){a="<generated>\n",b=p(e)}if(!b){a="<generated-ie>\n";var f=[];try{for(var h=arguments.callee.caller.caller;h&&f.length<c;){var i=E.test(h.toString())?RegExp.$1||d:d;f.push(i),h=h.caller}}catch(j){g(j)}b=f.join("\n")}return a+b}function p(a){return a.stack||a.backtrace||a.stacktrace}function q(a){var b={millisecondsAgo:new Date-a.timeStamp,type:a.type,which:a.which,target:r(a.target)};return b}function r(a){if(a){var b=a.attributes;if(b){for(var c="<"+a.nodeName.toLowerCase(),d=0;d<b.length;d++)b[d].value&&"null"!=b[d].value.toString()&&(c+=" "+b[d].name+'="'+b[d].value+'"');return c+">"}return a.nodeName}}function s(){z+=1,a.setTimeout(function(){z-=1})}function t(a,b,c){var d=a[b],e=c(d);a[b]=e}var u,v,w,x={},y=!0,z=0,A=10;x.noConflict=function(){return a.Bugsnag=b,x},x.refresh=function(){A=10},x.notifyException=function(a,b,c,d){b&&"string"!=typeof b&&(c=b,b=void 0),c||(c={}),f(c),n({name:b||a.name,message:a.message||a.description,stacktrace:p(a)||o(),file:a.fileName||a.sourceURL,lineNumber:a.lineNumber||a.line,columnNumber:a.columnNumber?a.columnNumber+1:void 0,severity:d||"warning"},c)},x.notify=function(b,c,d,e){n({name:b,message:c,stacktrace:o(),file:a.location.toString(),lineNumber:1,severity:e||"warning"},d)};var B="complete"!==document.readyState;document.addEventListener?(document.addEventListener("DOMContentLoaded",d,!0),a.addEventListener("load",d,!0)):a.attachEvent("onload",d);var C,D=/^[0-9a-f]{32}$/i,E=/function\s*([\w\-$]+)?\s*\(/i,F="https://notify.bugsnag.com/",G=F+"js",H="2.4.7",I=document.getElementsByTagName("script"),J=I[I.length-1];if(a.atob){if(a.ErrorEvent)try{0===new a.ErrorEvent("test").colno&&(y=!1)}catch(K){}}else y=!1;if(l("autoNotify",!0)){t(a,"onerror",function(b){return function(c,d,e,g,h){var i=l("autoNotify",!0),j={};!g&&a.event&&(g=a.event.errorCharacter),f(j),v=null,i&&!z&&n({name:h&&h.name||"window.onerror",message:c,file:d,lineNumber:e,columnNumber:g,stacktrace:h&&p(h)||o(),severity:"error"},j),b&&b(c,d,e,g,h)}});var L=function(a){return function(b,d){if("function"==typeof b){b=c(b);var e=Array.prototype.slice.call(arguments,2);return a(function(){b.apply(this,e)},d)}return a(b,d)}};t(a,"setTimeout",L),t(a,"setInterval",L),a.requestAnimationFrame&&t(a,"requestAnimationFrame",function(a){return function(b){return a(c(b))}}),a.setImmediate&&t(a,"setImmediate",function(a){return function(){var b=Array.prototype.slice.call(arguments);return b[0]=c(b[0]),a.apply(this,b)}}),"EventTarget Window Node ApplicationCache AudioTrackList ChannelMergerNode CryptoOperation EventSource FileReader HTMLUnknownElement IDBDatabase IDBRequest IDBTransaction KeyOperation MediaController MessagePort ModalWindow Notification SVGElementInstance Screen TextTrack TextTrackCue TextTrackList WebSocket WebSocketWorker Worker XMLHttpRequest XMLHttpRequestEventTarget XMLHttpRequestUpload".replace(/\w+/g,function(b){var d=a[b]&&a[b].prototype;d&&d.hasOwnProperty&&d.hasOwnProperty("addEventListener")&&(t(d,"addEventListener",function(a){return function(b,d,e,f){return d&&d.handleEvent&&(d.handleEvent=c(d.handleEvent,{eventHandler:!0})),a.call(this,b,c(d,{eventHandler:!0}),e,f)}}),t(d,"removeEventListener",function(a){return function(b,d,e,f){return a.call(this,b,d,e,f),a.call(this,b,c(d),e,f)}}))})}a.Bugsnag=x,"function"==typeof define&&define.amd?define([],function(){return x}):"object"==typeof module&&"object"==typeof module.exports&&(module.exports=x)}(window,window.Bugsnag);
Bugsnag.apiKey = "2a86b308af5a81d2c9329fedfb4b30c7";
Bugsnag.appVersion = "afbaf2f5c1da5beb066725eef2c0759f01547b6f-1429658093";
Bugsnag.endpoint = "https://errors.slack-core.com/js";
Bugsnag.releaseStage = "prod";
Bugsnag.user = {id:"U046VQQ96",name:"robertkety",email:"robertkety@gmail.com"};
Bugsnag.metaData = {};
Bugsnag.metaData.team = {id:"T046VQQ94",name:"cs419",domain:"cs419"};
Bugsnag.refresh_interval = setInterval(function () { (window.TS && window.TS.client) ? Bugsnag.refresh() : clearInterval(Bugsnag.refresh_interval); }, 15 * 60 * 1000);
</script>			<script type="text/javascript">

	(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
	(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
	m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
	})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
	ga('create', 'UA-106458-17', 'slack.com');
	ga('send', 'pageview');


	(function(e,c,b,f,d,g,a){e.SlackBeaconObject=d;
	e[d]=e[d]||function(){(e[d].q=e[d].q||[]).push([1*new Date(),arguments])};
	e[d].l=1*new Date();g=c.createElement(b);a=c.getElementsByTagName(b)[0];
	g.async=1;g.src=f;a.parentNode.insertBefore(g,a)
	})(window,document,"script","https://slack.global.ssl.fastly.net/dcf8/js/libs/beacon.js","sb");
	sb('set', 'token', '3307f436963e02d4f9eb85ce5159744c');
	sb('set', 'user_id', 'U046VQQ96');
	sb('set', 'user_batch', "ssb");
	sb('set', 'user_created', "2015-03-30");
	sb('set', 'name_tag', 'cs419/robertkety');
	sb('track', 'pageview');

	function track(a){ga('send','event','web',a);sb('track',a);}


	
	(function(f,b){if(!b.__SV){var a,e,i,g;window.mixpanel=b;b._i=[];b.init=function(a,e,d){function f(b,h){var a=h.split(".");2==a.length&&(b=b[a[0]],h=a[1]);b[h]=function(){b.push([h].concat(Array.prototype.slice.call(arguments,0)))}}var c=b;"undefined"!==typeof d?c=b[d]=[]:d="mixpanel";c.people=c.people||[];c.toString=function(b){var a="mixpanel";"mixpanel"!==d&&(a+="."+d);b||(a+=" (stub)");return a};c.people.toString=function(){return c.toString(1)+".people (stub)"};i="disable track track_pageview track_links track_forms register register_once alias unregister identify name_tag set_config people.set people.set_once people.increment people.append people.track_charge people.clear_charges people.delete_user".split(" ");
	for(g=0;g<i.length;g++)f(c,i[g]);b._i.push([a,e,d])};b.__SV=1.2;a=f.createElement("script");a.type="text/javascript";a.async=!0;a.src="//cdn.mxpnl.com/libs/mixpanel-2-latest.min.js";e=f.getElementsByTagName("script")[0];e.parentNode.insertBefore(a,e)}})(document,window.mixpanel||[]);
	
	mixpanel.init("12d52d8633a5b432975592d13ebd3f34");

	function mixpanel_track(event_name){if(window.mixpanel&&event_name)mixpanel.track(event_name);}

</script>	
</head>

  <body >

		  			<script>
		
			var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
			if (w > 1440) document.querySelector('body').classList.add('widescreen');
		
		</script>
	
  	
	

			<nav id="site_nav" class="no_transition">

	<div id="site_nav_contents">

		<div id="user_menu">
			<div id="user_menu_contents">
				<div id="user_menu_avatar">
										<span class="member_image thumb_48" style="background-image: url('https://secure.gravatar.com/avatar/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=192&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018.png')" data-thumb-size="48" data-member-id="U046VQQ96"></span>
					<span class="member_image thumb_36" style="background-image: url('https://secure.gravatar.com/avatar/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=72&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018-72.png')" data-thumb-size="36" data-member-id="U046VQQ96"></span>
				</div>
				<h3>Signed in as</h3>
				<span id="user_menu_name">robertkety</span>
			</div>
		</div>

		<div class="nav_contents">

			<ul class="primary_nav">
				<li><a href="/home"><i class="fa fa-home"></i>Home</a></li>
				<li><a href="/account"><i class="fa fa-user"></i>Account & Profile</a></li>
				<li><a href="/services/new"><i class="fa fa-wrench"></i>Integrations</a></li>
				<li><a href="/archives"><i class="fa fa-inbox"></i>Message Archives</a></li>
				<li><a href="/files"><i class="fa fa-file"></i>Files</a></li>
				<li><a href="/team"><i class="fa fa-book"></i>Team Directory</a></li>
									<li><a href="/stats"><i class="fa fa-tachometer"></i>Statistics</a></li>
													<li><a href="/customize"><i class="fa fa-magic"></i>Customize</a></li>
											</ul>

							<h3>Administration</h3>
				<ul id="admin_nav" class="secondary_nav">
					<li><a href="/admin/settings">Team Settings</a></li>
					<li><a href="/admin">Manage Your Team</a></li>
					<li><a href="/admin/invites">Invitations</a></li>
					<li><a href="/admin/billing">Billing</a></li>
					<li><a href="/admin/auth">Authentication</a></li>
				</ul>
			
		</div>

		<div id="footer">

			<ul id="footer_nav">
				<li><a href="/is">Tour</a></li>
				<li><a href="/apps">Apps</a></li>
				<li><a href="/brand-guidelines">Brand Guidelines</a></li>
				<li><a href="/help">Help</a></li>
				<li><a href="https://api.slack.com" target="_blank">API<i class="fa fa-external-link small_left_margin"></i></a></li>
									<li><a href="/account/gateways">Gateways</a></li>
								<li><a href="/pricing">Pricing</a></li>
				<li><a href="/help/requests/new">Contact</a></li>
				<li><a href="/terms-of-service">Policies</a></li>
				<li><a href="http://slackhq.com/" target="_blank">Our Blog</a></li>
				<li><a href="https://slack.com/signout/4233840310?crumb=s-1429673218-40c079c9df-%E2%98%83">Sign Out<i class="fa fa-sign-out small_left_margin"></i></a></li>
			</ul>

			<p id="footer_signature">Made with <i class="fa fa-heart"></i> by Slack</p>

		</div>

	</div>
</nav>	
	<header>
					<a id="menu_toggle" class="no_transition">
				<span class="menu_icon"></span>
				<span class="menu_label">Menu</span>
				<span class="vert_divider"></span>
			</a>
			<h1 id="header_team_name" class="inline_block no_transition">
				<a href="/home">
					<i class="fa fa-home" /></i>
					cs419
				</a>
			</h1>
			<div class="header_nav">
				<div class="header_btns float_right">
					<a id="team_switcher">
						<i class="fa fa-th-large"></i>
						<span class="block label">Teams</span>
					</a>
					<a href="/help" id="help_link">
						<i class="fa fa-life-ring"></i>
						<span class="block label">Help</span>
					</a>
					<a href="/messages">
						<img src="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-64.png" srcset="https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-32.png 1x, https://slack.global.ssl.fastly.net/0dc1/img/icons/ios-64.png 2x" />
						<span class="block label">Launch</span>
					</a>
				</div>
						                    <ul id="header_team_nav">
		                        		                            <li class="active">
		                            	<a href="https://cs419.slack.com/home" target="https://cs419.slack.com/">
		                            				                            			<i class="fa fa-check active_icon"></i>
		                            				                            				                            			<i class="team_icon default" >C</i>
		                            				                            		<span class="switcher_label team_name">cs419</span>
		                            	</a>
		                            </li>
		                        		                        <li id="add_team_option"><a href="https://slack.com/signin" target="_blank"><i class="fa fa-plus team_icon"></i> <span class="switcher_label">Sign in to another team...</span></a></li>
		                    </ul>
		                			</div>
		
		
	</header>

	<div id="page">

		<div id="page_contents" >

<p class="print_only">
	<strong>Created by ryford on April 17, 2015 at 5:06 PM</strong><br />
	<span class="subtle_silver break_word">https://cs419.slack.com/files/ryford/F04EHEERS/dbinsertion.sql</span>
</p>

<div class="file_header_container no_print"></div>

<div class="alert_container">
		<div class="file_public_link_shared alert" style="display: none;">
			<a id="file_public_link_revoker" class="btn btn_small btn_outline float_right" data-toggle="tooltip" title="You can revoke the public link to this file. This will cause any previously shared links to stop working.">Revoke</a>
		
	<i class="fa fa-link"></i> Public Link: <a class="file_public_link" href="https://slack-files.com/T046VQQ94-F04EHEERS-b02f120180" target="new">https://slack-files.com/T046VQQ94-F04EHEERS-b02f120180</a>
</div></div>

<div id="file_page" class="card top_padding">

	<p class="small subtle_silver no_print meta">
		53KB SQL snippet created on <span class="date">April 17th 2015</span>.
				<span class="file_share_list"></span>
	</p>

	<a id="file_action_cog" class="action_cog action_cog_snippet float_right no_print">
		<span>Actions </span><i class="fa fa-cog"></i>
	</a>
	<a id="snippet_expand_toggle" class="float_right no_print">
		<i class="fa fa-expand "></i>
		<i class="fa fa-compress hidden"></i>
	</a>

	<div class="large_bottom_margin clearfix">
		<pre id="file_contents">USE thegoods;

INSERT INTO crrd_organization VALUES
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Albany-Corvallis ReUseIt&quot;, NULL, NULL, NULL, NULL, NULL, &quot;groups.yahoo.com&quot;, &quot;Free items&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Arc Thrift Store&quot;, &quot;541-754-9011&quot;, &quot;928 NW Beca Ave&quot;, NULL, NULL, &quot;97330&quot;, &quot;www.arcbenton.org&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Arc Thrift Store&quot;, &quot;541-929-3946&quot;, &quot;936 Main St.&quot;, NULL, NULL, &quot;97370&quot;, &quot;www.arcbenton.org&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Beekman Place Antique Mall&quot;, &quot;541-753-8250&quot;, &quot;601 SW Western Blvd&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Benton County Extension / 4-H  Activities&quot;, &quot;541-766-6750&quot;, &quot;1849 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Benton County Master Gardeners&quot;, &quot;541-766-6750&quot;, &quot;1849 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Book Bin&quot;, &quot;541-752-0040&quot;, &quot;215 SW 4th St&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.bookbin.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Browsers&#039; Bookstore&quot;, &quot;541-758-1121&quot;, &quot;121 NW 4th St.&quot;, NULL, NULL, &quot;97330&quot;, &quot;www.browsersbookstore.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Boys &amp; Girls Club / STARS&quot;, &quot;541-757-1909&quot;, NULL, NULL, NULL, NULL, NULL, &quot;After school program&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Buckingham Palace&quot;, &quot;541-752-7980&quot;, &quot;600 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, &quot;Friday - Sunday only&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;CARDV (Center Against Rape/Domestic Violence)&quot;, &quot;541-758-0219&quot;, NULL, NULL, NULL, NULL, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Career Closet for Women&quot;, &quot;541-754-6979&quot;, &quot;942 NW 9th&quot;, &quot;Ste.A&quot;, NULL, &quot;97330&quot;, NULL, &quot;Address is for drop off location&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cat&#039;s Meow Humane Society Thrift Shop&quot;, &quot;541-757-0573&quot;, &quot;411 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Children&#039;s Farm Home&quot;, &quot;541-757-1852&quot;, &quot;4455 NE Hwy 20&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Chintimini Wildlife Rehabilitation Ctr&quot;, &quot;541-745-5324&quot;, &quot;311 Lewisburg Rd&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Community Outreach Homeless Shelter&quot;, &quot;541-758-3000&quot;, &quot;865 NW Reiman Ave&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Corvallis Environmental Center&quot;, &quot;541-753-9211&quot;, &quot;214 SW Monroe Ave&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Corvallis Bicycle Collective&quot;, &quot;541-224-6885&quot;, &quot;33900 SE Roche Ln&quot;, &quot;Unit B&quot;, NULL, &quot;97333&quot;, &quot;www.corvallisbikes.org/&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Corvallis Furniture&quot;, &quot;541-231-8103&quot;, &quot;720 NE Granger Ave&quot;, &quot;Bldg J&quot;, NULL, &quot;97330&quot;, &quot;www.corvallisfurniture.com/&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Corvallis-Uzhhorod Sister Cities/The TOUCH Project&quot;, &quot;541-753-5170&quot;, NULL, NULL, NULL, NULL, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cosmic Chameleon&quot;, &quot;541-752-9001&quot;, &quot;138 SW 2nd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Craigslist&quot;, NULL, NULL, NULL, NULL, NULL, &quot;corvallis.craigslist.org&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Freecycle&quot;, NULL, NULL, NULL, NULL, NULL, &quot;freecycle.org&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;First Alternative Co-op Recycling Center&quot;, &quot;541-753-3115&quot;, &quot;1007 SE 3rd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;First Alternative Co-op Store&quot;, &quot;541-452-3115&quot;, &quot;2855 NW Grant Ave&quot;, NULL, NULL, &quot;97330&quot;, &quot;www.firstalt.coop&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;First Alternative Co-op Store&quot;, &quot;541-753-3115&quot;, &quot;1007 SE 3rd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.firstalt.coop&quot;, NULL),
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Furniture Exchange&quot;, &quot;541-833-0183&quot;, &quot;210 NW 2nd St&quot;, NULL, NULL, &quot;97330&quot;, &quot;www.furnitureexchange-usa.com/&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Furniture Share&quot;, &quot;541-754-9511&quot;, &quot;155 SE Lilly Ave&quot;, NULL, NULL, &quot;97333&quot;, NULL, &quot;Formerly Benton FS&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Garland Nursery&quot;, &quot;541-753-6601&quot;, &quot;5470 NE Hwy 20&quot;, NULL, NULL, &quot;97330&quot;, &quot;www.garlandnursery.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Home Grown Gardens&quot;, &quot;541-758-2137&quot;, &quot;4845 SE 3rd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.homegrowngardens.biz&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Goodwill Industries&quot;, &quot;541-752-8278&quot;, &quot;1325 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Habitat for Humanity ReStore&quot;, &quot;541-752-6637&quot;, &quot;4840 SW Philomath Blvd&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Happy Trails Records, Tapes &amp; CDs&quot;, &quot;541-752-9032&quot;, &quot;100 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Heartland Humane Society&quot;, &quot;541-757-9000&quot;, &quot;398 SW Twin Oaks Cir&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.heartlandhuman.org&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Jackson Street Youth Shelter&quot;, &quot;541-754-2404&quot;, &quot;555 NW Jackson St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Linn Benton Food Share&quot;, &quot;541-752-1010&quot;, &quot;545 SW 2nd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, &quot;Lg food donations&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lions Club&quot;, &quot;541-758-0222&quot;, &quot;1400 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, &quot;Box inside Elks Lodge&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Love INC&quot;, &quot;541-757-8111&quot;, &quot;2330 NW Professional Dr&quot;, NULL, NULL, &quot;97330&quot;, NULL, &quot;For low income citizens&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Mario Pastega House&quot;, &quot;541-768-4650&quot;, &quot;3505 NW Samaritan Dr&quot;, NULL, NULL, &quot;97330&quot;, NULL, &quot;Good samaritan patient family housing&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Mary&#039;s River Gleaners&quot;, &quot;541-752-1010&quot;, NULL, NULL, NULL, NULL, NULL, &quot;For low income citizens&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Midway Farms&quot;, &quot;541-740-6141&quot;, &quot;6980 NW Hwy 20&quot;, NULL, NULL, &quot;97321&quot;, &quot;www.midwayfarmsoregon.com/&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Neighbor to Neighbor&quot;, &quot;541-929-6614&quot;, &quot;1123 Main St&quot;, NULL, NULL, &quot;97370&quot;, NULL, &quot;Food pantry&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Osborn Aquatic Center&quot;, &quot;541-766-7946&quot;, &quot;1940 NW Highland Dr&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;OSU Emergency Food Pantry&quot;, &quot;541-737-3473&quot;, &quot;2150 SW Jefferson Way&quot;, NULL, NULL, &quot;97331&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;OSU Folk Club Thrift Shop&quot;, &quot;541-752-4733&quot;, &quot;144 NW 2nd St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;OSU Organic Growers Club&quot;, &quot;541-737-6810&quot;, NULL, NULL, NULL, NULL, NULL, &quot;Crop &amp; Soil Science Department&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Pak Mail&quot;, &quot;541-754-8411&quot;, &quot;2397 NW Kings Blvd&quot;, NULL, NULL, &quot;97330&quot;, &quot;pakmail.com&quot;, &quot;Timberhill Shopping Center&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Parent Enhancement Program&quot;, &quot;541-758-8292&quot;, &quot;421 NW 4th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Pastors for Peace-Caravan to Cuba&quot;, &quot;541-754-1858&quot;, NULL, NULL, NULL, NULL, NULL, &quot;Contact Mike Beilstein&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Philomath Community Garden&quot;, &quot;541-929-3524&quot;, NULL, NULL, NULL, NULL, NULL, &quot;Contact Chris Shonnard&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Philomath Community Services&quot;, &quot;541-929-2499&quot;, &quot;360 S 9th St&quot;, NULL, NULL, &quot;97370&quot;, NULL, &quot;Food &amp; kids stuff&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Play It Again Sports&quot;, &quot;541-754-7529&quot;, &quot;1422 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Presbyterian Piecemakers&quot;, &quot;541-753-7516&quot;, NULL, NULL, NULL, NULL, NULL, &quot;Cotton quilts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Public Library Corvallis, Friends of&quot;, &quot;541-766-6928&quot;, &quot;645 NW Monroe Ave&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Quilts From Caring Hands&quot;, &quot;541-758-8161&quot;, NULL, NULL, NULL, NULL, NULL, &quot;Cotton quilts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Rapid Refill Ink&quot;, &quot;541-758-8444&quot;, &quot;254 SW Madison Ave&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.rapidinkandtoner.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Office Max&quot;, &quot;541-738-0990&quot;, &quot;1834 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Replay Children&#039;s Wear&quot;, &quot;541-753-6903&quot;, &quot;260 NW 1st St&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Re•volve&quot;, &quot;541-754-1154&quot;, &quot;103 SW 2nd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.revolveresale.com/&quot;, &quot;Women&#039;s resale boutique&quot;),
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Schools--Public, Private, Charter&quot;, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Second Glance&quot;, &quot;541-753-8011&quot;, &quot;312 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.glanceagain.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Second Glance Annex&quot;, &quot;541-758-9099&quot;, &quot;214 SW Jefferson Ave&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.glanceagain.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Second Glance Alley&quot;, &quot;541-753-4069&quot;, &quot;312 SW Jefferson Ave&quot;, NULL, NULL, &quot;97333&quot;, &quot;www.glanceagain.com&quot;, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Senior Center of Corvallis&quot;, &quot;541-766-6959&quot;, &quot;2601 NW Tyler Ave&quot;, NULL, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;South Corvallis Food Bank&quot;, &quot;541-753-4263&quot;, &quot;1798 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Stone Soup &amp; St Vincent de Paul&quot;, &quot;541-757-1988&quot;, &quot;501 NW 25th St&quot;, NULL, NULL, &quot;97330&quot;, NULL, &quot;St Mary&#039;s Church&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;The UPS Store&quot;, &quot;541-752-1830&quot;, &quot;5060 SW Philomath Blvd&quot;, NULL, NULL, &quot;97333&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;The UPS Store&quot;, &quot;541-752-0056&quot;, &quot;922 NW Circle Blvd&quot;, &quot;#160&quot;, NULL, &quot;97330&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Vina Moses&quot;, &quot;541-753-1420&quot;, &quot;969 NW Garfield&quot;, NULL, NULL, &quot;97330&quot;, NULL, &quot;For low income citizens&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Vintage House Parts&quot;, &quot;541-740-9513&quot;, &quot;135 N 13th St&quot;, NULL, NULL, &quot;97370&quot;, NULL, NULL),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Book Binding&quot;, &quot;541-757-9861&quot;, &quot;108 SW 3rd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;http://www.cornerstoneassociates.com/bj-bookbinding-about-us.html&quot;, &quot;Rebind and restore books&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cell Phone Sick Bay&quot;, &quot;541-230-1785&quot;, &quot;252 SW Madison Ave&quot;, &quot;Suite 10&quot;, NULL, &quot;97333&quot;, NULL, &quot;Cell phones and tablets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Geeks &#039;N&#039; Nerds&quot;, &quot;541-753-0018&quot;, &quot;950 Southeast Geary St&quot;, &quot;Suite 110&quot;, NULL, &quot;97333&quot;, &quot;http://www.computergeeksnnerds.com/&quot;, &quot;Repair computers of all kinds and cell phone repair. In home repair available&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Specialty Sewing By Leslie&quot;, &quot;541-758-4556&quot;, &quot;225 SW Madison Ave&quot;, NULL, NULL, &quot;97333&quot;, &quot;http://www.specialtysewing.com/Leslie_Seamstress/Welcome.html&quot;, &quot;Alterations and custom work&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Corvallis Technical&quot;, &quot;541-704-7009&quot;, &quot;966 NW Circle Blvd&quot;, NULL, NULL, &quot;97330&quot;, &quot;http://www.corvallistechnical.com/&quot;, &quot;Repair computers and laptops&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bellevue Computers&quot;, &quot;541-757-3487&quot;, &quot;1865 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, &quot;http://www.bellevuepc.com/&quot;, &quot;Repair computers and laptops&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;OSU Repair Fair&quot;, &quot;541-737-5398&quot;, &quot;Oregon State University Property Services Building&quot;, &quot;644 SW 13th St&quot;, NULL, &quot;97333&quot;, &quot;http://fa.oregonstate.edu/surplus&quot;, &quot;Occurs twice per quarter in the evenings. Small appliances, Bicycles, Clothing, Computers (hardware and software) Electronics (small items only) Housewares (furniture, ceramics, lamps, etc.)&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;P.K Furniture Repair &amp; Refinishing&quot;, &quot;541-230-1727&quot;, &quot;5270 NW Hwy 99&quot;, NULL, NULL, &quot;97330&quot;, &quot;http://www.pkfurniturerefinishing.net/&quot;, &quot;Complete Restoration, Complete Refinishing, Modifications, Custom Color Matching, Furniture Stripping,Chair Press Caning, Repairs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Furniture Restoration Center&quot;, &quot;541-929-6681&quot;, &quot;1321 Main St&quot;, NULL, NULL, &quot;97370&quot;, &quot;http://restorationsupplies.com/&quot;, &quot;Restores all typers of furniture and has hardware for doing it yourself&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Power Equipment&quot;, &quot;541-757-8075&quot;, &quot;713 NE Circle Blvd&quot;, NULL, NULL, &quot;97330&quot;, &quot;https://corvallispowerequipment.stihldealer.net/&quot;, &quot;Lawn and garden equipment, chain saws (Stihl, Honda, Shindiawh), hand held equipment.&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Robnett&#039;s&quot;, &quot;541-753-5531&quot;, &quot;400 SW 2nd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;http://ww3.truevalue.com/robnetts/Home.aspx&quot;, &quot;Adjustment and sharpening&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Footwise&quot;, &quot;541-757-0875&quot;, &quot;301 SW Madison Ave&quot;, &quot;#100&quot;, NULL, &quot;97333&quot;, &quot;http://footwise.com/&quot;, &quot;Resoles berkenstock&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sedlack&quot;, &quot;541-752-1498&quot;, &quot;225 SW 2nd St&quot;, NULL, NULL, &quot;97333&quot;, &quot;http://www.sedlaksshoes.net/&quot;, &quot;Full resoles, elastic and velcros, sewing and patching, leather patches, zippers, half soles and heels.&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Foam Man&quot;, &quot;541-754-9378&quot;, &quot;2511 NW 9th St&quot;, NULL, NULL, &quot;97330&quot;, &quot;http://www.thefoammancorvallis.com/&quot;, &quot;Replacement foam cusions for chairs and couches; upholstery&quot;);
    
INSERT INTO crrd_category VALUES
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Household&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bedding/Bath&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Children&#039;s Goods&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Appliances - Small&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Appliances - Large&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Building/Home Improvement&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Wearables&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Useable Electronics&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sporting Equipment/Camping&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Garden&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Food&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Medical Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Office Equipment&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Packing Materials&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Miscellaneous&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Repair Items&quot;);
    
INSERT INTO crrd_item VALUES
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Arts and Crafts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Barbeque Grills&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Books&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Canning Jars&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cleaning Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Clothes Hangers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cookware&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Dishes&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Fabric&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Food Storage Containers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Furniture&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Luggage&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Mattresses&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Ornaments&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Toiletries&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Utensils&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Blankets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Comforters&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Linens&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sheets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Small Rugs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Towels&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Baby Carriers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Baby Gates&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bike Trailers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Child Car Seats&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Clothes&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Crayons&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cribs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Diapers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;High Chairs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Maternity&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Musical Instruments&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Nursing Items&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Playpens&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;School Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Strollers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Toys&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Blenders&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Dehumidifiers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Fans&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Microwaves&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Space Heaters&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Toasters&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Vacuum Cleaners&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Dishwashers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Freezers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Refrigerators&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Stoves&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Washers/Dryers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bricks&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Carpet Padding&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Carpets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Ceramic Tiles&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Doors&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Drywall&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Electrical Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hand Tools&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hardware&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Insulation&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Ladders&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Light Fixtures&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lighting Ballasts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lumber&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Motors&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Paint&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Pipe&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Plumbing&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Power Tools&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Reusable Metal Items&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Roofing&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Vinyl&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Windows&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Belts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Boots&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Coats&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hats&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Rainwear&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sandals&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Shoes&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Calculators&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cameras&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cassette Players&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;CD Players&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;CDs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Cell Phones&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Computers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Curling Irons&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;DVD Players&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Game Consoles&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;GPS Systems&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hair Dryers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Monitors&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;MP3 Players&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Printers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Projectors&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Receivers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Scanners&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Speakers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Tablets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Telephones&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;TVs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Backpacks&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Balls&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Barbells&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bicycles&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bike Tires&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Camping Equipment&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Day Packs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Dumbbells&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Exercise Equipment&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Golf Clubs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Helmets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hiking Boots&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Skateboards&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Skis&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Small Boats&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Snowshoes&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sporting Goods&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Tennis Rackets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Tents&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Chain Saws&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Fencing&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Garden Pots&quot;),
	(REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Garden Tools&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hand Clippers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hoses&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lawn Furniture&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Livestock Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Loppers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Mowers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Seeders&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Soil Amendment&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Sprinklers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Wheel Barrows&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Beverages&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Surplus Garden Produce&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Unopened Canned Goods&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Unopened Packaged Food&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Adult Diapers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Blood Pressure Monitors&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Canes&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Crutches&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Eye Glasses&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Glucose Meters&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hearing Aids&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Hospital Beds&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Reach Extenders&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Shower Chairs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Walkers&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Wheelchairs&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Fax Machines&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Headsets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Office Furniture&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Paper Shredders&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Printer Cartridge&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Bubble Wrap&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Clean Foam Peanuts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Foam Sheets&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Egg Cartons&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Firewood&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Paper Bags&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Pet Supplies&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Shopping Bags&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Vehicles/Parts&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Computer Paper&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Small Appliances&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lamps&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Lawn Power Equipment&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Outdoor Gear&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Shoes/Boots&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Upholstery, Car&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Upholstery, Furniture&quot;),
    (REPLACE(uuid(),&#039;-&#039;,&#039;&#039;), &quot;Screen Repair&quot;);

INSERT INTO crrd_item_category VALUES
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Arts and Crafts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Barbeque Grills&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Books&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Canning Jars&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cleaning Supplies&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clothes Hangers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cookware&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Dishes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fabric&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Food Storage Containers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Furniture&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Luggage&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Mattresses&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Ornaments&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Toiletries&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Household&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Utensils&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Blankets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Comforters&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Linens&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Sheets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Small Rugs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Bedding/Bath&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Towels&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Arts and Crafts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Baby Carriers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Baby Gates&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bike Trailers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Books&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Child Car Seats&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clothes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Crayons&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cribs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Diapers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;High Chairs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Maternity&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Musical Instruments&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Nursing Items&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Playpens&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;School Supplies&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Strollers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Children&#039;s Goods&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Toys&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Blenders&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Dehumidifiers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fans&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Microwaves&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Space Heaters&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Toasters&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Small&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Vacuum Cleaners&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Large&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Dishwashers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Large&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Freezers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Large&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Refrigerators&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Large&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Stoves&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Appliances - Large&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Washers/Dryers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bricks&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Carpet Padding&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Carpets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Ceramic Tiles&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Doors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Drywall&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Electrical Supplies&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hand Tools&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hardware&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Insulation&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Ladders&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Light Fixtures&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lighting Ballasts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lumber&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Motors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Paint&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Pipe&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Plumbing&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Power Tools&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Reusable Metal Items&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Roofing&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Vinyl&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Building/Home Improvement&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Windows&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Belts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Boots&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clothes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Coats&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hats&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Rainwear&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Sandals&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Wearables&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Shoes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Calculators&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cameras&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cassette Players&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;CD Players&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;CDs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cell Phones&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Curling Irons&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;DVD Players&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Game Consoles&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;GPS Systems&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hair Dryers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Monitors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;MP3 Players&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Printers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Projectors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Receivers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Scanners&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Speakers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Tablets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Telephones&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Useable Electronics&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;TVs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Backpacks&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Balls&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Barbells&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bicycles&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bike Tires&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Camping Equipment&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Day Packs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Dumbbells&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Exercise Equipment&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Golf Clubs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Helmets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hiking Boots&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Skateboards&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Skis&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Small Boats&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Snowshoes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Sporting Goods&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Tennis Rackets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Sporting Equipment/Camping&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Tents&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Chain Saws&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fencing&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Garden Pots&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Garden Tools&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hand Clippers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hoses&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lawn Furniture&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Livestock Supplies&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Loppers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Mowers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Seeders&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Soil Amendment&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Sprinklers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Garden&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Wheel Barrows&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Food&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Beverages&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Food&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Surplus Garden Produce&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Food&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Unopened Canned Goods&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Food&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Unopened Packaged Food&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Adult Diapers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Blood Pressure Monitors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Canes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Crutches&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Eye Glasses&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Glucose Meters&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hearing Aids&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Hospital Beds&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Reach Extenders&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Shower Chairs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Walkers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Medical Supplies&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Wheelchairs&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Calculators&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fax Machines&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Headsets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Monitors&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Office Furniture&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Paper Shredders&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Printer Cartridge&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Printers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Scanners&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Office Equipment&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Telephones&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Packing Materials&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bubble Wrap&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Packing Materials&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clean Foam Peanuts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Packing Materials&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Foam Sheets&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Egg Cartons&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Firewood&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fabric&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Paper Bags&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Pet Supplies&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Shopping Bags&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Vehicles/Parts&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computer Paper&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Miscellaneous&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Reusable Metal Items&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cell Phones&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Small Appliances&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Books&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clothes&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Furniture&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lamps&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lawn Power Equipment&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Outdoor Gear&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Screen Repair&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Shoes/Boots&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Upholstery, Car&quot;)),
    (null, (SELECT cat_id FROM crrd_category WHERE cat_name = &quot;Repair Items&quot;), (SELECT item_id FROM crrd_item WHERE item_name = &quot;Upholstery, Furniture&quot;));
    
INSERT INTO crrd_repairable VALUES
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Books&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Book Binding&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cell Phones&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Cell Phone Sick Bay&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Cell Phones&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Geeks &#039;N&#039; Nerds&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clothes&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Specialty Sewing By Leslie&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Corvallis Technical&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Bellevue Computers&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Geeks &#039;N&#039; Nerds&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;OSU Repair Fair&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Small Appliances&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;OSU Repair Fair&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Furniture&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;P.K Furniture Repair &amp; Refinishing&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Furniture&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Furniture Restoration Center&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lawn Power Equipment&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Power Equipment&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Lawn Power Equipment&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Robnett&#039;s&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Sandals&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Footwise&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Screen Repair&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Robnett&#039;s&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Shoes/Boots&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Sedlack&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Upholstery, Furniture&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Foam Man&quot;));
    
INSERT INTO crrd_reusable VALUES
	(null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Calculators&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Computers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Fax Machines&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Headsets&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Monitors&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Office Furniture&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Paper Shredders&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Printers&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Scanners&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Telephones&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Bubble Wrap&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Clean Foam Peanuts&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Foam Sheets&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Pet Supplies&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
	(null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;School Supplies&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
	(null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Toiletries&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;)),
    (null, (SELECT item_id FROM crrd_item WHERE item_name = &quot;Vehicles/Parts&quot;), (SELECT org_id FROM crrd_organization WHERE org_name = &quot;Albany-Corvallis ReUseIt&quot;));</pre>

		<p class="file_page_meta no_print" style="line-height: 1.5rem;">
			<label class="checkbox normal mini float_right no_top_padding no_min_width">
				<input type="checkbox" id="file_preview_wrap_cb"> wrap long lines
			</label>
		</p>

	</div>

	<div id="comments_holder" class="clearfix clear_both">
	<div class="col span_1_of_6"></div>
	<div class="col span_4_of_6 no_right_padding">
		<div id="file_page_comments">
			<div class="loading_hash_animation">
	<img src="https://slack.global.ssl.fastly.net/f85a/img/loading_hash_animation_@2x.gif" srcset="https://slack.global.ssl.fastly.net/272a/img/loading_hash_animation.gif 1x, https://slack.global.ssl.fastly.net/f85a/img/loading_hash_animation_@2x.gif 2x" alt="Loading" class="loading_hash" /><br />loading...
	<noscript>
		You must enable javascript in order to use Slack :(
				<style type="text/css">div.loading_hash { display: none; }</style>
	</noscript>
</div>		</div>	
		<form action="https://cs419.slack.com/files/ryford/F04EHEERS/dbinsertion.sql" 
		id="file_comment_form" 
					class="comment_form"
				method="post">
			<a href="/team/robertkety" class="member_preview_link" data-member-id="U046VQQ96" >
			<span class="member_image thumb_36" style="background-image: url('https://secure.gravatar.com/avatar/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=72&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018-72.png')" data-thumb-size="36" data-member-id="U046VQQ96"></span>
		</a>		
		<input type="hidden" name="addcomment" value="1" />
	<input type="hidden" name="crumb" value="s-1429673218-b2c06dedd0-☃" />

	<textarea id="file_comment" data-el-id-to-keep-in-view="file_comment_submit_btn" class="comment_input small_bottom_margin" name="comment" wrap="virtual" ></textarea>
	<span class="input_note float_left cloud_silver file_comment_tip">cmd+enter to submit</span>	<button id="file_comment_submit_btn" type="submit" class="btn float_right  ladda-button" data-style="expand-right"><span class="ladda-label">Add Comment</span></button>
</form>

<form action="https://cs419.slack.com/files/ryford/F04EHEERS/dbinsertion.sql" 
		id="file_edit_comment_form" 
					class="edit_comment_form hidden"
				method="post">
	<textarea id="file_edit_comment" class="comment_input small_bottom_margin" name="comment" wrap="virtual"></textarea><br>
	<span class="input_note float_left cloud_silver file_comment_tip">cmd+enter to submit</span>	<input type="submit" class="save btn float_right " value="Save Changes" />
	<button class="cancel btn btn_outline float_right small_right_margin ">Cancel</button>
</form>	
	</div>
	<div class="col span_1_of_6"></div>
</div>
</div>

	

		
	</div>
	<div id="overlay"></div>
</div>




<script type="text/javascript">
var cdn_url = 'https://slack.global.ssl.fastly.net';
</script>
			<script type="text/javascript">
<!--
	// common boot_data
	var boot_data = {
		start_ms: new Date().getTime(),
		app: 'web',
		is_mobile: false,
		user_id: 'U046VQQ96',
		version_ts: '1429658093',
		version_uid: 'afbaf2f5c1da5beb066725eef2c0759f01547b6f',
		redir_domain: 'slack-redir.net',
		api_url: '/api/',
		team_url: 'https://cs419.slack.com/',
		image_proxy_url: 'https://slack-imgs.com/',
		api_token: 'xoxs-4233840310-4233840312-4543542210-187d21f94f',
		feature_status: false,
		feature_attachments_inline: false,
		feature_search_attachments: false,
		feature_chat_sounds: false,
		feature_cmd_autocomplete: true,
		feature_require_at: true,
		feature_image_proxy: true,
		feature_channel_eventlog_client: true,
		feature_bot_users: true,
		feature_post_previews: false,
		feature_user_hidden_msgs: false,
		feature_muting: true,
		feature_macssb1_banner: true,
		feature_winssb1_banner: true,
		feature_latest_event_ts: true,
		feature_no_redirects_in_ssb: true,
		feature_referer_policy: true,
		feature_client_exif_orientation_on_uploads: true,
		feature_spaces: false,
		feature_lato_fonts: true,
		feature_at_channel_warning: true,
		feature_at_channel_warning_non_admin_message: true,
		feature_flexpane_rework: true,
		feature_ms_on_space: true,
		feature_a11y_keyboard_shortcuts: false,
		feature_email_ingestion: false,
		feature_email_integration: false,
		feature_pins: true,
		feature_join_leave_rollups: true,
		feature_prompt_to_share: false,
		feature_bot_message_label: true,
		feature_file_url_private_conversion: false,
		feature_spaces_in_windows: false,
		feature_oldest_msg_storing: true,
		feature_reactions: false,
		feature_archive_viewer: true,
		feature_new_btns_in_channel_list: true,
		feature_screenhero: false,
		feature_pricing_page_refresh: false,
		feature_client_date_formatting: false,
		feature_more_field_in_message_attachments: false,
		feature_fix_files: false,
		feature_box_plugin: false,
		feature_combined_menu: false,

		img: {
			app_icon: 'https://slack.global.ssl.fastly.net/272a/img/slack_growl_icon.png'
		},
		page_needs_custom_emoji: false
	};

	

	// client boot data
			boot_data.login_data = JSON.parse('{\"ok\":true,\"self\":{\"id\":\"U046VQQ96\",\"name\":\"robertkety\",\"prefs\":{\"highlight_words\":\"\",\"user_colors\":\"\",\"color_names_in_list\":true,\"growls_enabled\":true,\"tz\":\"America\\/Los_Angeles\",\"push_dm_alert\":true,\"push_mention_alert\":true,\"push_everything\":true,\"push_idle_wait\":2,\"push_sound\":\"b2.mp3\",\"push_loud_channels\":\"\",\"push_mention_channels\":\"\",\"push_loud_channels_set\":\"\",\"email_alerts\":\"instant\",\"email_alerts_sleep_until\":0,\"email_misc\":false,\"email_weekly\":true,\"welcome_message_hidden\":false,\"all_channels_loud\":true,\"loud_channels\":\"\",\"never_channels\":\"\",\"loud_channels_set\":\"\",\"show_member_presence\":true,\"search_sort\":\"timestamp\",\"expand_inline_imgs\":true,\"expand_internal_inline_imgs\":true,\"expand_snippets\":false,\"posts_formatting_guide\":true,\"seen_welcome_2\":true,\"seen_ssb_prompt\":false,\"search_only_my_channels\":false,\"emoji_mode\":\"default\",\"emoji_use\":\"{\\\"disappointed\\\":1}\",\"has_invited\":true,\"has_uploaded\":true,\"has_created_channel\":true,\"search_exclude_channels\":\"\",\"messages_theme\":\"default\",\"webapp_spellcheck\":true,\"no_joined_overlays\":false,\"no_created_overlays\":false,\"dropbox_enabled\":false,\"seen_user_menu_tip_card\":true,\"seen_team_menu_tip_card\":true,\"seen_channel_menu_tip_card\":true,\"seen_message_input_tip_card\":true,\"seen_channels_tip_card\":true,\"seen_domain_invite_reminder\":false,\"seen_member_invite_reminder\":false,\"seen_flexpane_tip_card\":true,\"seen_search_input_tip_card\":true,\"mute_sounds\":false,\"arrow_history\":false,\"tab_ui_return_selects\":true,\"obey_inline_img_limit\":true,\"new_msg_snd\":\"knock_brush.mp3\",\"collapsible\":false,\"collapsible_by_click\":true,\"require_at\":false,\"mac_ssb_bounce\":\"\",\"mac_ssb_bullet\":true,\"expand_non_media_attachments\":true,\"show_typing\":true,\"pagekeys_handled\":true,\"last_snippet_type\":\"\",\"display_real_names_override\":0,\"time24\":false,\"enter_is_special_in_tbt\":false,\"graphic_emoticons\":false,\"convert_emoticons\":true,\"autoplay_chat_sounds\":true,\"ss_emojis\":true,\"sidebar_behavior\":\"\",\"mark_msgs_read_immediately\":true,\"start_scroll_at_oldest\":true,\"snippet_editor_wrap_long_lines\":false,\"ls_disabled\":false,\"sidebar_theme\":\"default\",\"sidebar_theme_custom_values\":\"\",\"f_key_search\":false,\"k_key_omnibox\":true,\"speak_growls\":false,\"mac_speak_voice\":\"com.apple.speech.synthesis.voice.Alex\",\"mac_speak_speed\":250,\"comma_key_prefs\":false,\"at_channel_suppressed_channels\":\"\",\"push_at_channel_suppressed_channels\":\"\",\"prompted_for_email_disabling\":false,\"full_text_extracts\":false,\"no_text_in_notifications\":false,\"muted_channels\":\"\",\"no_macssb1_banner\":false,\"no_winssb1_banner\":true,\"privacy_policy_seen\":true,\"search_exclude_bots\":false,\"fuzzy_matching\":false,\"load_lato_2\":false,\"fuller_timestamps\":false,\"last_seen_at_channel_warning\":0,\"enable_flexpane_rework\":false,\"flex_resize_window\":false,\"msg_preview\":false,\"msg_preview_displaces\":true,\"msg_preview_persistent\":true,\"emoji_autocomplete_big\":false,\"winssb_run_from_tray\":true,\"two_factor_auth_enabled\":false,\"mentions_exclude_at_channels\":true,\"box_enabled\":false},\"created\":1427756221},\"team\":{\"id\":\"T046VQQ94\",\"name\":\"cs419\",\"email_domain\":\"onid.oregonstate.edu,oregonstate.edu\",\"domain\":\"cs419\",\"msg_edit_window_mins\":-1,\"prefs\":{\"default_channels\":[\"C046VQQ9E\",\"C046VQQ9L\"],\"gateway_allow_xmpp_ssl\":0,\"gateway_allow_irc_ssl\":1,\"gateway_allow_irc_plain\":1,\"msg_edit_window_mins\":-1,\"allow_message_deletion\":true,\"hide_referers\":true,\"display_real_names\":false,\"who_can_at_everyone\":\"regular\",\"who_can_at_channel\":\"ra\",\"warn_before_at_channel\":\"always\",\"who_can_create_channels\":\"regular\",\"who_can_archive_channels\":\"regular\",\"who_can_create_groups\":\"ra\",\"who_can_post_general\":\"ra\",\"who_can_kick_channels\":\"admin\",\"who_can_kick_groups\":\"regular\",\"retention_type\":0,\"retention_duration\":0,\"group_retention_type\":0,\"group_retention_duration\":0,\"dm_retention_type\":0,\"dm_retention_duration\":0,\"require_at_for_mention\":0,\"compliance_export_start\":0},\"icon\":{\"image_34\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-34.png\",\"image_44\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-44.png\",\"image_68\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-68.png\",\"image_88\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-88.png\",\"image_102\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-102.png\",\"image_132\":\"https:\\/\\/slack.global.ssl.fastly.net\\/b3b7\\/img\\/avatars-teams\\/ava_0016-132.png\",\"image_default\":true},\"over_storage_limit\":false,\"plan\":\"\"},\"latest_event_ts\":\"1429672618.000000\",\"channels\":[{\"id\":\"C049BH0BA\",\"name\":\"database\",\"is_channel\":true,\"created\":1428346598,\"creator\":\"U046VQQ96\",\"is_archived\":false,\"is_general\":false,\"is_member\":true,\"members\":[\"U046VQQ96\",\"U048VCGMA\",\"U0490APNC\"],\"topic\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0},\"purpose\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0}},{\"id\":\"C046VQQ9E\",\"name\":\"general\",\"is_channel\":true,\"created\":1427756221,\"creator\":\"U046VQQ96\",\"is_archived\":false,\"is_general\":true,\"is_member\":true,\"members\":[\"U046VQQ96\",\"U048VCGMA\",\"U0490APNC\"],\"topic\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0},\"purpose\":{\"value\":\"This channel is for team-wide communication and announcements. All team members are in this channel.\",\"creator\":\"\",\"last_set\":0}},{\"id\":\"C047KDYPM\",\"name\":\"github-commits\",\"is_channel\":true,\"created\":1427756392,\"creator\":\"U046VQQ96\",\"is_archived\":false,\"is_general\":false,\"is_member\":true,\"members\":[\"U046VQQ96\",\"U048VCGMA\",\"U0490APNC\"],\"topic\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0},\"purpose\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0}},{\"id\":\"C046VQQ9L\",\"name\":\"random\",\"is_channel\":true,\"created\":1427756221,\"creator\":\"U046VQQ96\",\"is_archived\":false,\"is_general\":false,\"is_member\":true,\"members\":[\"U046VQQ96\",\"U048VCGMA\",\"U0490APNC\"],\"topic\":{\"value\":\"\",\"creator\":\"\",\"last_set\":0},\"purpose\":{\"value\":\"A place for non-work-related flimflam, faffing, hodge-podge or jibber-jabber you\'d prefer to keep out of more focused work-related channels.\",\"creator\":\"\",\"last_set\":0}}],\"groups\":[],\"ims\":[{\"id\":\"D046VQQ98\",\"is_im\":true,\"user\":\"USLACKBOT\",\"created\":1427756221,\"is_user_deleted\":false},{\"id\":\"D048VCGMG\",\"is_im\":true,\"user\":\"U048VCGMA\",\"created\":1428175485,\"is_user_deleted\":false},{\"id\":\"D0490APPU\",\"is_im\":true,\"user\":\"U0490APNC\",\"created\":1428258192,\"is_user_deleted\":false}],\"users\":[{\"id\":\"U0490APNC\",\"name\":\"mikecamilleri\",\"deleted\":false,\"status\":null,\"color\":\"e7392d\",\"real_name\":\"Michael Camilleri\",\"tz\":\"America\\/Los_Angeles\",\"tz_label\":\"Pacific Daylight Time\",\"tz_offset\":-25200,\"profile\":{\"first_name\":\"Michael\",\"last_name\":\"Camilleri\",\"real_name\":\"Michael Camilleri\",\"real_name_normalized\":\"Michael Camilleri\",\"email\":\"camillmi@onid.oregonstate.edu\",\"image_24\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/e419ee9b566630f6d622bfdde0184f8e.jpg?s=24&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0016-24.png\",\"image_32\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/e419ee9b566630f6d622bfdde0184f8e.jpg?s=32&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0016-32.png\",\"image_48\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/e419ee9b566630f6d622bfdde0184f8e.jpg?s=48&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0016-48.png\",\"image_72\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/e419ee9b566630f6d622bfdde0184f8e.jpg?s=72&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0016-72.png\",\"image_192\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/e419ee9b566630f6d622bfdde0184f8e.jpg?s=192&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0016.png\"},\"is_admin\":false,\"is_owner\":false,\"is_primary_owner\":false,\"is_restricted\":false,\"is_ultra_restricted\":false,\"is_bot\":false},{\"id\":\"U046VQQ96\",\"name\":\"robertkety\",\"deleted\":false,\"status\":null,\"color\":\"9f69e7\",\"real_name\":\"Robert Kety\",\"tz\":\"America\\/Los_Angeles\",\"tz_label\":\"Pacific Daylight Time\",\"tz_offset\":-25200,\"profile\":{\"first_name\":\"Robert\",\"last_name\":\"Kety\",\"phone\":\"5034811357\",\"real_name\":\"Robert Kety\",\"real_name_normalized\":\"Robert Kety\",\"email\":\"robertkety@gmail.com\",\"image_24\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=24&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018-24.png\",\"image_32\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=32&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018-32.png\",\"image_48\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=48&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F272a%2Fimg%2Favatars%2Fava_0018-48.png\",\"image_72\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=72&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018-72.png\",\"image_192\":\"https:\\/\\/secure.gravatar.com\\/avatar\\/43c0d1c613de4115699fad3da0f4ad4f.jpg?s=192&d=https%3A%2F%2Fslack.global.ssl.fastly.net%2F3654%2Fimg%2Favatars%2Fava_0018.png\"},\"is_admin\":true,\"is_owner\":true,\"is_primary_owner\":true,\"is_restricted\":false,\"is_ultra_restricted\":false,\"is_bot\":false},{\"id\":\"U048VCGMA\",\"name\":\"ryford\",\"deleted\":false,\"status\":null,\"color\":\"4bbe2e\",\"real_name\":\"Ryan Ford\",\"tz\":\"America\\/Los_Angeles\",\"tz_label\":\"Pacific Daylight Time\",\"tz_offset\":-25200,\"profile\":{\"first_name\":\"Ryan\",\"last_name\":\"Ford\",\"image_24\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_24.jpg\",\"image_32\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_32.jpg\",\"image_48\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_48.jpg\",\"image_72\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_72.jpg\",\"image_192\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_192.jpg\",\"image_original\":\"https:\\/\\/s3-us-west-2.amazonaws.com\\/slack-files2\\/avatars\\/2015-04-04\\/4325260467_9b430b60f80623bb8904_original.jpg\",\"real_name\":\"Ryan Ford\",\"real_name_normalized\":\"Ryan Ford\",\"email\":\"fordr@onid.oregonstate.edu\"},\"is_admin\":false,\"is_owner\":false,\"is_primary_owner\":false,\"is_restricted\":false,\"is_ultra_restricted\":false,\"is_bot\":false},{\"id\":\"USLACKBOT\",\"name\":\"slackbot\",\"deleted\":false,\"status\":null,\"color\":\"757575\",\"real_name\":\"Slack Bot\",\"tz\":null,\"tz_label\":\"Pacific Daylight Time\",\"tz_offset\":-25200,\"profile\":{\"first_name\":\"Slack\",\"last_name\":\"Bot\",\"image_24\":\"https:\\/\\/slack-assets2.s3-us-west-2.amazonaws.com\\/10068\\/img\\/slackbot_24.png\",\"image_32\":\"https:\\/\\/slack-assets2.s3-us-west-2.amazonaws.com\\/10068\\/img\\/slackbot_32.png\",\"image_48\":\"https:\\/\\/slack-assets2.s3-us-west-2.amazonaws.com\\/10068\\/img\\/slackbot_48.png\",\"image_72\":\"https:\\/\\/slack-assets2.s3-us-west-2.amazonaws.com\\/10068\\/img\\/slackbot_72.png\",\"image_192\":\"https:\\/\\/slack-assets2.s3-us-west-2.amazonaws.com\\/10068\\/img\\/slackbot_192.png\",\"real_name\":\"Slack Bot\",\"real_name_normalized\":\"Slack Bot\",\"email\":null},\"is_admin\":false,\"is_owner\":false,\"is_primary_owner\":false,\"is_restricted\":false,\"is_ultra_restricted\":false,\"is_bot\":false}],\"bots\":[{\"id\":\"B046VRJU0\",\"name\":\"github\",\"deleted\":false,\"icons\":{\"image_48\":\"https:\\/\\/slack.global.ssl.fastly.net\\/5721\\/plugins\\/github\\/assets\\/bot_48.png\"}},{\"id\":\"B047KFH8X\",\"name\":\"hangouts\",\"deleted\":false,\"icons\":{\"image_48\":\"https:\\/\\/slack.global.ssl.fastly.net\\/6f04\\/img\\/services\\/hangouts_48.png\"}}],\"svn_rev\":\"dev\",\"min_svn_rev\":99999,\"version_ts\":1429658093,\"min_version_ts\":1429207025,\"cache_version\":\"v6-dog\"}');
	
//-->
</script>			
			
		
	<!-- output_js "core" -->
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/c304/js/rollup-core_required.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/c212/js/libs/bootstrap_plastic.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/bf3b/js/libs/fastclick.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/8556/js/libs/headroom.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/8827/js/plastic.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/6e65/js/TS.web.js" crossorigin="anonymous"></script>

			<!-- output_js "secondary" -->
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/84fb/js/rollup-secondary_required.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/33c4/js/TS.storage.js" crossorigin="anonymous"></script>

		<!-- output_js "regular" -->
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/8528/js/TS.web.comments.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/056f/js/TS.web.file.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/2eab/js/libs/codemirror.js" crossorigin="anonymous"></script>
<script type="text/javascript" src="https://slack.global.ssl.fastly.net/9e78/js/codemirror_load.js" crossorigin="anonymous"></script>

		<script type="text/javascript">
	<!--
		boot_data.page_needs_custom_emoji = true;
		
		boot_data.file = JSON.parse('{\"id\":\"F04EHEERS\",\"created\":1429315599,\"timestamp\":1429315599,\"name\":\"DBInsertion.sql\",\"title\":\"Initial Insertion for Database Tables.sql\",\"mimetype\":\"text\\/plain\",\"filetype\":\"sql\",\"pretty_type\":\"SQL\",\"user\":\"U048VCGMA\",\"editable\":true,\"size\":54711,\"mode\":\"snippet\",\"is_external\":false,\"external_type\":\"\",\"is_public\":true,\"public_url_shared\":false,\"url\":\"https:\\/\\/slack-files.com\\/files-pub\\/T046VQQ94-F04EHEERS-b02f120180\\/dbinsertion.sql\",\"url_download\":\"https:\\/\\/slack-files.com\\/files-pub\\/T046VQQ94-F04EHEERS-b02f120180\\/download\\/dbinsertion.sql\",\"url_private\":\"https:\\/\\/files.slack.com\\/files-pri\\/T046VQQ94-F04EHEERS\\/dbinsertion.sql\",\"url_private_download\":\"https:\\/\\/files.slack.com\\/files-pri\\/T046VQQ94-F04EHEERS\\/download\\/dbinsertion.sql\",\"permalink\":\"https:\\/\\/cs419.slack.com\\/files\\/ryford\\/F04EHEERS\\/dbinsertion.sql\",\"permalink_public\":\"https:\\/\\/slack-files.com\\/T046VQQ94-F04EHEERS-b02f120180\",\"edit_link\":\"https:\\/\\/cs419.slack.com\\/files\\/ryford\\/F04EHEERS\\/dbinsertion.sql\\/edit\",\"preview\":\"USE thegoods;\\n\\nINSERT INTO crrd_organization VALUES\\n\\t(REPLACE(uuid(),\'-\',\'\'), \\\"Albany-Corvallis ReUseIt\\\", NULL, NULL, NULL, NULL, NULL, \\\"groups.yahoo.com\\\", \\\"Free items\\\"),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Arc Thrift Store\\\", \\\"541-754-9011\\\", \\\"928 NW Beca Ave\\\", NULL, NULL, \\\"97330\\\", \\\"www.arcbenton.org\\\", NULL),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Arc Thrift Store\\\", \\\"541-929-3946\\\", \\\"936 Main St.\\\", NULL, NULL, \\\"97370\\\", \\\"www.arcbenton.org\\\", NULL),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Beekman Place Antique Mall\\\", \\\"541-753-8250\\\", \\\"601 SW Western Blvd\\\", NULL, NULL, \\\"97333\\\", NULL, NULL),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Benton County Extension \\/ 4-H  Activities\\\", \\\"541-766-6750\\\", \\\"1849 NW 9th St\\\", NULL, NULL, \\\"97330\\\", NULL, NULL),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Benton County Master Gardeners\\\", \\\"541-766-6750\\\", \\\"1849 NW 9th St\\\", NULL, NULL, \\\"97330\\\", NULL, NULL),\\n    (REPLACE(uuid(),\'-\',\'\'), \\\"Book Bin\\\", \\\"541-752-0040\\\", \\\"215 SW 4th St\\\", NULL, NULL, \\\"97333\\\", \\\"www.bookbin.com\\\", NULL),\",\"preview_highlight\":\"<div class=\\\"sssh-code\\\"><div class=\\\"sssh-line\\\"><pre><span class=\\\"sssh-keyword\\\">USE<\\/span> thegoods;<\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre><span class=\\\"sssh-keyword\\\">INSERT<\\/span> <span class=\\\"sssh-keyword\\\">INTO<\\/span> crrd_organization <span class=\\\"sssh-keyword\\\">VALUES<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>\\t<span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Albany-Corvallis ReUseIt&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;groups.yahoo.com&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Free items&quot;<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Arc Thrift Store&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-754-9011&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;928 NW Beca Ave&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97330&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;www.arcbenton.org&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Arc Thrift Store&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-929-3946&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;936 Main St.&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97370&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;www.arcbenton.org&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Beekman Place Antique Mall&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-753-8250&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;601 SW Western Blvd&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97333&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Benton County Extension \\/ 4-H  Activities&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-766-6750&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;1849 NW 9th St&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97330&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Benton County Master Gardeners&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-766-6750&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;1849 NW 9th St&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97330&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<div class=\\\"sssh-line\\\"><pre>    <span>&#40;<\\/span><span class=\\\"sssh-keyword\\\">REPLACE<\\/span><span>&#40;<\\/span>uuid<span>&#40;<\\/span><span>&#41;<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'-\'<\\/span><span>,<\\/span><span class=\\\"sssh-string\\\">\'\'<\\/span><span>&#41;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;Book Bin&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;541-752-0040&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;215 SW 4th St&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;97333&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-string\\\">&quot;www.bookbin.com&quot;<\\/span><span>,<\\/span> <span class=\\\"sssh-keyword\\\">NULL<\\/span><span>&#41;<\\/span><span>,<\\/span><\\/pre><\\/div>\\n<\\/div>\",\"lines\":511,\"lines_more\":501,\"channels\":[\"C049BH0BA\"],\"groups\":[],\"ims\":[],\"comments_count\":1,\"initial_comment\":{\"id\":\"Fc04F5CZJ5\",\"created\":1429315599,\"timestamp\":1429315599,\"user\":\"U048VCGMA\",\"comment\":\"Note that the table names are prepended with \\\"cddr\\\", which was the name of the app I used. Simply perform a find and replace to change \\\"cddr\\\" with whatever the name is of the app used in the official project. \"}}');
		boot_data.file.comments = JSON.parse('[{\"id\":\"Fc04F5CZJ5\",\"created\":1429315599,\"timestamp\":1429315599,\"user\":\"U048VCGMA\",\"comment\":\"Note that the table names are prepended with \\\"cddr\\\", which was the name of the app I used. Simply perform a find and replace to change \\\"cddr\\\" with whatever the name is of the app used in the official project. \"}]');

		

		var g_editor;

		$(function(){

			var wrap_long_lines = !!TS.model.code_wrap_long_lines;

			g_editor = CodeMirror(function(elt){
				var content = document.getElementById("file_contents");
				content.parentNode.replaceChild(elt, content);
			}, {
				value: $('#file_contents').text(),
				lineNumbers: true,
				matchBrackets: true,
				indentUnit: 4,
				indentWithTabs: true,
				enterMode: "keep",
				tabMode: "shift",
				viewportMargin: Infinity,
				readOnly: true,
				lineWrapping: wrap_long_lines
			});

			$('#file_preview_wrap_cb').bind('change', function(e) {
				TS.model.code_wrap_long_lines = $(this).prop('checked');
				g_editor.setOption('lineWrapping', TS.model.code_wrap_long_lines);
			})

			$('#file_preview_wrap_cb').prop('checked', wrap_long_lines);

			CodeMirror.switchSlackMode(g_editor, 'sql');
		});

		
		$('#file_comment').css('overflow', 'hidden').autogrow();
	//-->
	</script>

			<script type="text/javascript">TS.boot(boot_data);</script>
	<!-- slack-www238 / 2015-04-21 20:26:58 / vafbaf2f5c1da5beb066725eef2c0759f01547b6f -->

</body>
</html>
<!doctype html>
<html>
<head>
<title>	<g:layoutTitle default="Referencerator by Appire Alliance" /></title>
	<link rel="stylesheet" href="${resource(dir:'css',file:'main.css')}" />
	<link rel="shortcut icon" href="${resource(dir:'images',file:'appireIcon2.ico')}" type="image/x-icon" />
	<g:layoutHead/>
	<g:javascript library="application" />
</head>
<body>
	<div id="spinner" class="spinner" style="display: none;">
		<img src="${resource(dir:'images',file:'spinner.gif')}" alt="${message(code:'spinner.alt',default:'Loading...')}" />
	</div>
	<div id="referenceratorLogo">
		<div id="loginArea">
			<sec:ifNotLoggedIn>
				<form action='/referencerator/j_spring_security_check' method='POST' id='loginForm' class='cssform'>
					username: <input type='text' class='text_' name='j_username' id='username' style='margin-left: 0;' /> 
					password: <input type='password' class='text_' name='j_password' id='password' />
					<input type='submit' value='Login' class='login'><br>
					<div id='utilities'>
						<g:link controller="register" action="forgotPassword">Forgot Password</g:link>
						<g:link controller="register">Register</g:link>
					</div>
					</form>
					<div class='clear'></div>
			</sec:ifNotLoggedIn>
			<sec:ifLoggedIn>
					<g:link controller="logout">
						<sec:username /> Logout</g:link>
			</sec:ifLoggedIn>
		</div>
	</div>
	<div id="nav">
		<div class="navPagePanel">
			<div class="panelTop">
				<h1>Navigate The Site</h1>
			</div>
			<div class="panelBody">
				<a href="${resource(dir: '/')}">Home Page</a> 
				<g:link controller="team">Meet the team</g:link>
				<g:link controller="download">Downloads</g:link>
				<g:link controller="tutorial">Tutorials</g:link>
				<g:link controller="underconstruction">Forums</g:link>
				<g:link controller="ContactUs">Contact Us</g:link>
			</div>
			<div class="panelBottom"></div>
		</div>
	</div>
	<g:layoutBody />
</body>
</html>
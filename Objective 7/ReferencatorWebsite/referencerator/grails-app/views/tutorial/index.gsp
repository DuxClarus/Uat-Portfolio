<html>
<head>
<link rel="stylesheet"
       href="${resource(dir:'css',file:'tutorialstyle.css')}" />
<title>List of Tutorials</title>
<meta name="layout" content="main" />
</head>
<body>
       <sec:ifNotLoggedIn>
               <div id="notLoggedIn">
                       <p>You must login before you access this page. If you don't have an account please register. To login enter your username and password in the above fields</p>
               </div>
       </sec:ifNotLoggedIn>
       <sec:ifAllGranted roles="ROLE_ADMIN">
       <div id="pageBody">
               <div id="table">
                       <g:def var="counter" value="${0}" />
                       <table>
                               <g:each in="${tutorials}" var="tutorial">
                                       <tr>
                                               <td style="background-color:${counter%2 == 0 ? '#c0c0c0' : '#000000'} ">
                                               <g:set var="counter" value="${counter++ }" />
                                                       <h1>Title of tutorial: ${tutorial.title}</h1>
                                                       <h2>Description of tutorial: ${tutorial.description }</h2>
                                                       <h3><a href="http://www.youtube.com"> Url to video: ${tutorial.urlToVideo }</a></h3>
                                               </td>
                                       </tr>
                               </g:each>
                       </table>
               </div>
       </div>
</sec:ifAllGranted>
</body>
</html>
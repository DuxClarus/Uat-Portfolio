package referencerator
import org.codehaus.groovy.grails.plugins.springsecurity.SpringSecurityUtils

class LogoutController {

	/**
	 * Index action. Redirects to the Spring security logout uri.
	 */
	def index = {
		flash.message = "Good-bye!"
		redirect uri: SpringSecurityUtils.securityConfig.logout.filterProcessesUrl // '/j_spring_security_logout'
	}
}

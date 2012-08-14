package referencerator

import org.springframework.dao.DataIntegrityViolationException

class TutorialController {
	def springSecurityService
	
	def tutorial = {
		def user = SecUser.get(springSecurityService.principal.id)
	}
		
	def index = {
		[tutorials: Tutorial.findAll()]
	}
	
	String toString(){
		name
	}
}
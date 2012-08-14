//be sure to import all used domain classes
import referencerator.SecUser
import referencerator.SecRole
import referencerator.SecUserSecRole
import referencerator.Tutorial

class BootStrap {

	def init = { servletContext ->
		println "Creating tutorials."
		createTutorials()
		println "Done. Now creating forum topics."
		createForums()
		println "Done. Now creating roles."
		createRoles()
		println "Done. Now creating admin account."
		createAdminUser()
		println "Done."
	}
	
	def destroy = {
	}
	
	def createForums() {

	}
	
	void createRoles() {
		if (!SecRole.count()) {
			new SecRole(authority: "ROLE_ADMIN").save(failOnError: true)
			new SecRole(authority: "ROLE_USER").save(failOnError: true)
			new SecRole(authority: "ROLE_GUEST").save(failOnError: true)
		}
	}

	def createAdminUser() {
		def userRole = SecRole.findByAuthority('ROLE_USER') ?: new SecRole(authority: 'ROLE_USER').save(failOnError: true)
		def adminRole = SecRole.findByAuthority('ROLE_ADMIN') ?: new SecRole(authority: 'ROLE_ADMIN').save(failOnError: true)
		def adminUser = SecUser.findByUsername('admin') ?: new SecUser(
				username: 'admin',
				password: 'admin',
				enabled: true).save(failOnError: true)

		if (!adminUser.authorities.contains(adminRole)) {
			SecUserSecRole.create adminUser, adminRole
		}
	}
	
	def createTutorials() {
		new Tutorial(title: "How to pose a model", description: "This video will teach you to pose a model in the Referencerator Application.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "How to place and re-size a shape", description: "This video will teach you to add an object to your scene and how to resize the object in the Referencerator Application.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "How to add lights", description: "This video will teach you how to add lights to your scene in the Referencerator Application.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "How to add props", description: "This video will teach you how to add lights to your scene in the Referencerator Application.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "examples", description: "This video will teach you how to examples.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "we love examples", description: "This video will teach you how examples.", urlToVideo: "youtube.com").save(failOnError: true)
		new Tutorial(title: "examples!!!", description: "This video will teach you how to examples.", urlToVideo: "youtube.com").save(failOnError: true)
		}
}

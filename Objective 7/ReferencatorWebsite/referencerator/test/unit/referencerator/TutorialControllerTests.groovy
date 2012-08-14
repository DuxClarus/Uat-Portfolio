package referencerator



import org.junit.*
import grails.test.mixin.*

@TestFor(TutorialController)
@Mock(Tutorial)
class TutorialControllerTests {


    def populateValidParams(params) {
      assert params != null
      // TODO: Populate valid properties like...
      //params["name"] = 'someValidName'
    }

    void testIndex() {
        controller.index()
        assert "/tutorial/list" == response.redirectedUrl
    }

    void testList() {

        def model = controller.list()

        assert model.tutorialInstanceList.size() == 0
        assert model.tutorialInstanceTotal == 0
    }

    void testCreate() {
       def model = controller.create()

       assert model.tutorialInstance != null
    }

    void testSave() {
        controller.save()

        assert model.tutorialInstance != null
        assert view == '/tutorial/create'

        response.reset()

        populateValidParams(params)
        controller.save()

        assert response.redirectedUrl == '/tutorial/show/1'
        assert controller.flash.message != null
        assert Tutorial.count() == 1
    }

    void testShow() {
        controller.show()

        assert flash.message != null
        assert response.redirectedUrl == '/tutorial/list'


        populateValidParams(params)
        def tutorial = new Tutorial(params)

        assert tutorial.save() != null

        params.id = tutorial.id

        def model = controller.show()

        assert model.tutorialInstance == tutorial
    }

    void testEdit() {
        controller.edit()

        assert flash.message != null
        assert response.redirectedUrl == '/tutorial/list'


        populateValidParams(params)
        def tutorial = new Tutorial(params)

        assert tutorial.save() != null

        params.id = tutorial.id

        def model = controller.edit()

        assert model.tutorialInstance == tutorial
    }

    void testUpdate() {
        controller.update()

        assert flash.message != null
        assert response.redirectedUrl == '/tutorial/list'

        response.reset()


        populateValidParams(params)
        def tutorial = new Tutorial(params)

        assert tutorial.save() != null

        // test invalid parameters in update
        params.id = tutorial.id
        //TODO: add invalid values to params object

        controller.update()

        assert view == "/tutorial/edit"
        assert model.tutorialInstance != null

        tutorial.clearErrors()

        populateValidParams(params)
        controller.update()

        assert response.redirectedUrl == "/tutorial/show/$tutorial.id"
        assert flash.message != null

        //test outdated version number
        response.reset()
        tutorial.clearErrors()

        populateValidParams(params)
        params.id = tutorial.id
        params.version = -1
        controller.update()

        assert view == "/tutorial/edit"
        assert model.tutorialInstance != null
        assert model.tutorialInstance.errors.getFieldError('version')
        assert flash.message != null
    }

    void testDelete() {
        controller.delete()
        assert flash.message != null
        assert response.redirectedUrl == '/tutorial/list'

        response.reset()

        populateValidParams(params)
        def tutorial = new Tutorial(params)

        assert tutorial.save() != null
        assert Tutorial.count() == 1

        params.id = tutorial.id

        controller.delete()

        assert Tutorial.count() == 0
        assert Tutorial.get(tutorial.id) == null
        assert response.redirectedUrl == '/tutorial/list'
    }
}

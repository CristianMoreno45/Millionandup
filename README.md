# Millionandup

Es una aplicación de tipo web api la cual está montada en C# .Net 6 orientada a microservicios según el estándar de **eShopOnContainers** de Microsoft. 

**[Disclaimer]**: Dadas a sus limitadas funcionalidades y la intención per se del proyecto el cual es es netamente académica, todo lo expuesto aquí es ficticio y es para fines demostrativos, no está en contenedores, no cuneta con servicios de autorregistro, autodescubrimiento, balanceo de carga, broker de eventos entre otros como si está **eShopOnContainers**. 

## Arquitectura de la solución
```	
├── MsIdentityServer (Encargado de la autenticación y autorización)
│   ├── Millionandup.MsIdentityServer (Microservicio de Identity server 4)
├── Millionandup.MsProperty (Encargado de la autenticación y autorización)
│   ├── Millionandup.MsProperty.Api (Capa de servicios REST)
│   ├── Millionandup.MsProperty.Domain  (Capa de dominio DDD)
│   ├── Millionandup.MsProperty.Infrastructure  (Capa de datos)
│   ├── Millionandup.MsProperty.Test (Proyecto de pruebas)
├── Millionandup.Framework (Herramientas transversales a toda la solución)
├── Millionandup.Framework.Domain (Herramientas transversales a toda la solución para la capa de dominio)
└── Millionandup.Framework.Repository (Herramientas transversales a toda la solución para la capa de datos)
```

## Generalidades

**Estructura de Identity server 4**
El microservicio de Identity Server no está dividido en capas (ya que su naturaleza no lo requiere) y cuenta con Controladores clásicos.

**Minimal API**
Por otro lado, las APIs del microservicio de propiedades están construidas en **Minimal API** una novedad de .Net 6 que permite crear APIs más rápido y con una lectura más estructurada. El Proyecto **Millionandup.MsProperty.Api** cuenta con dos carpetas **Endpoints** la cual es responsable de los edpoints y **EndpointsHandlers** la cual es responsable del comportamiento.

**Ejemplo de un Endpoint**

```
public static partial class PropertyApi
{

	private const string API_BASE_PATH = "api/Property/v1";

	public static void AddPropertyEndpoints(this WebApplication app)
	{
		app.MapPost($"{API_BASE_PATH}/CreatePropertyBuilding", PropertyHandlers.CreatePropertyBuilding).RequireAuthorization("CreatePropertyBuilding");
		app.MapPost($"{API_BASE_PATH}/AddImageFromProperty", PropertyHandlers.AddImageFromProperty).RequireAuthorization("AddImageFromProperty");
	}
}
```

**Ejemplo de un EndpointHandler**

```
public static partial class PropertyHandlers
{

	internal static async Task<IResult> CreatePropertyBuilding([FromServices] IProperty propertyService, [FromServices] ILogger logger, [FromBody] CreatePropertyBuildingRequest property)
	{
		try
		{
			return Results.Json(propertyService.CreatePropertyBuilding(property).AsResponseDTO());
		}
		catch (InvalidModelException ex)
		{
			logger.LogError(AppLogEvents.Error, ex, $"[{DateTime.UtcNow}] - {ex.Message}" );
			return Results.Json(false.AsResponseDTO(HttpStatusCode.BadRequest, ex.Message), statusCode: (int)HttpStatusCode.BadRequest);
		}
		catch (Exception ex)
		{
			logger.LogError(AppLogEvents.Error, ex, ex.Message);
			return Results.Json(false.AsResponseDTO(HttpStatusCode.InternalServerError, MessagesError.GENERAL_ERROR), statusCode: (int)HttpStatusCode.InternalServerError);
		}

	}
}
```
**Repository pattern**

También se aplicó el patrón de repositorio para desacoplar El modelo de datos del modelo de negocio, por lo que se cuenta con un dominio, su interfaz, la interfaz del repositorio y el repositorio propiamente dicho. 
P. ej.: 
* **Property (Dominio)**: Responsable de la lógica de negocio.
* **IProperty (Interfaz del dominio)**: Contrato del dominio, tanto de propiedades como métodos.
* **IPropertyRespositry (Interfaz del repositorio)**: Contrato de acceso a datos, esta no tiene propiedades, pero si los métodos básicos de CRUD.
* **PropertyRespository (Repositorio)**: responsable del acceso a datos.

**Diagrama MER**
![Diagrama MER*](https://github.com/CristianMoreno45/Millionandup/blob/master/Doc/MillionandupRepository.drawio.png?raw=true "Diagrama MER*")

## Pruebas Unitarias
El framework escogido fue NUnit nutrido con un modelo de datos a base de Mocks , cuenta con las tres etapas clave de pruebas unitarias Arreglar (Arrange), Actuar (Act) y Afirmar (Assert) y esta centrado en la capa de Negocio, validando así si el funcionamiento de las reglas del dominio. 

**Estructura**

Los proyectos de tipo Test están estructurados según **{{Proyecto base}}Tests**. 
P. ej.: **Millionandup.MsProperty.Test**

Los nombres las clase de prueba esta estructurados como **{{nombre del dominio}}_{{método del dominio}}Tests**. 
P. ej.: **Property_AddImageFromPropertyTests**

Los nombres de los métodos de prueba están estructurados según **{{Resultado esperado}}_{{Caso de prueba}}**. 
P. ej.: **Property_AddImageFromPropertyTests**

**Ejemplo de un Mock**

```	
// Modelo de datos
IQueryable<Property>? mock = _propertyList.BuildMock();
// Mock
var modelMock = new Mock<IPropertyModel>(); 
// Comporamiento esperado frente a una consulta al modelo de datos al metodo GetAll()
modelMock.Setup(b => b.GetAll()).Returns(mock); 
// Comporamiento esperado frente a una consulta al modelo de datos al metodo GetByFilter(... ciertos estimulos...)
modelMock.Setup(x => x.GetByFilter(x => x.Name.ToUpper() == name.ToUpper()))
	.Returns(_propertyList.Where(x => x.Name.ToUpper() == name.ToUpper()).AsQueryable());
return modelMock;
```
**[Opinión personal]**: Hoy en día muchas organizaciones desdibujan la naturaleza de la prueba unitaria dejando a un lado otro tipo de pruebas que complementan el proceso de calidad y desarrollo de software, como por ejemplo las pruebas automatizadas funcionales las cuales validan los casos de negocio clave y que de alguna manera son repetitivos; las pruebas manuales de los nuevos desarrollos en el ciclo de vida del desarrollo; pruebas automatizadas no funcionales como por ejemplo carga, estrés, disponibilidad entre otros atributos de arquitectura; las pruebas de seguridad como la penetración,  suplantación, Input fuzzing entre otras más.  
Las pruebas deben ser atómicas, independientes y rápidas.

## Modelo de validación

Se hace a atreves del framework **Fluent Valdiation** el cual flexibiliza mucho la creación de pruebas [Sitio oficial] (https://docs.fluentvalidation.net/en/latest/).
Los nombres las clase de prueba esta estructurados como **{{nombre del dominio}}Validator**.
P. ej.: **PropertyImageValidator**

Las clases de este tipo están dentro de la capa de negocio en la carpeta **ModelValidation** y cuentan con las 4 partes:
* Definición de constantes (generalmente son expresiones regulares).
* Constructor el cual define el conjunto de reglas de acuerdo a un nombre (puede ser CREATE, UPDATE u otro que se defina).
* Métodos de afirmación, donde se quiere que por medio de una condición arroje un booleano el cual indique si se ha roto alguna regla de negocio.
* Mensajes de Error como contantes (son muy útiles para centralizar mensajes de excepción y para las pruebas unitarias).

**Ejemplo de un set de reglas de negocio**

```	
RuleFor(p => p.Name).Must(name => IsNullOrEmpty(name)).WithMessage(MessagesError.NAME_IS_MANDATORY);
RuleFor(p => p.Name).Matches(REG_LETTERS_NUMBERS_AND_SPACE).WithMessage(MessagesError.NAME_INCORRECT_FORMAT);
RuleFor(p => p.Name.Length).LessThan(80).WithMessage(string.Format(MessagesError.MAX_LENGHT_STRING, "Name",80));
RuleFor(p => p.Price).GreaterThan(0).WithMessage(MessagesError.PRICE_GREATER_THAN_ZERO);
```	


## Autenticación y autorización 
Para ello se usa **Millionandup.MsIdentityServer** el cual es un servicio que permite la autenticación y autorización, ideal para la gestión de usuarios, claves, roles, scopes entre otros [Sitio oficial]( https://identityserver4.readthedocs.io/en/latest/).


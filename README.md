[![Build Status](https://travis-ci.org/DeepFocus/uTyped.svg?branch=master)](https://travis-ci.org/DeepFocus/uTyped)

This package helps you get read access to your Umbraco data in a strongly typed way.  
This can be useful when you need to returns some of your content through an API controller for example.

## Get the package ##

You can install the package from [nuget](https://www.nuget.org/packages/uTyped) using the following command in the Package Manager Console:

`Install-Package uTyped`

## Using the default mapping

The easiest and quickest way to get setup is to use AutoMapper (this library is already installed with Umbraco).  
You will have to create your Models first. For example:

	public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
	
And then once the application has started, you can add the AutoMapper configuration:

    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        //Initialize AutoMapper
        Mapper.AddProfile<UmbracoProfile<Product>>();
        
        // Do not use the following line because it might break your back office:
        // Mapper.Initialize(cfg => cfg.AddProfile<UmbracoProfile<Product>>());
    }
	
We are using a profile in this example but this isn't mandatory. You could as well create your map and add the resolver for all properties:

    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        //Initialize AutoMapper
		Mapper.CreateMap<IPublishedContent, Product>()
			.ForAllMembers(opt => opt.ResolveUsing<PropertyResolver>());
    }
	
Once the setup is done, you can access your data from your controller:

	public class MyController : UmbracoApiController
    {
        private readonly UmbracoRepository _repo;

        public TestController()
        {
            _repo = new UmbracoRepository(Umbraco);
        }

        public IEnumerable<Product> GetAll()
        {
            return _repo.GetAll<Product>();
        }

        public Product Get(int id)
        {
            return _repo.GetById<Product>(id);
        }
    }
	
Now if you go to http://yourwebsite/umbraco/api/test/getall you should be able to see all your products.

## More information

By default, `_repo.GetAll<T>()` creates the xPath using the name of the class. If your data is nested, you can pass a different xPath:

	public IEnumerable<Product> GetAll()
	{
		return _repo.GetAll<Product>("//Data/Products");
	}
	
## Using a different mapping

All the methods can take a custom `Func<IPublishedContent, T>()` to allow you to skip AutoMapper if you feel like it.

## Resolvers

We've added a [bunch of resolvers](https://github.com/DeepFocus/uType/tree/master/uTyped/Resolvers) we often use to help you.  
For example, if your product Model is upgraded to:

	public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string[] Images { get; set; }
    }
	
You will have to use the `MultiMediaResolver` for the `Images` property.  
Using the AutoMapper profile:

    public class ProductProfile : UmbracoProfile<Product>
    {
        private readonly UmbracoHelper _umbracoHelper;

        public ProductProfile(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, Product>()
                .ForMember(dest => dest.Images, opt => opt.ResolveUsing(new MultiMediaResolver(_umbracoHelper)));

            base.Configure();
        }
    }
	
And once again initialize AutoMapper:

    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        //Initialize AutoMapper
        Mapper.AddProfile<ProductProfile>();
    }

# SpecificationExtensions

This repository contains two projects

#### - t0pez.SpecificationExtensions.Core
#### - t0pez.SpecificationExtensions.Generators

These two packages adds a few extensions for Ardalis.Specification package. 
Using these packages gives you an opportunity make you specifications more reusable

## Package SpecificationExtensions.Core 
Provides few base specifications and extesions for them

### Class `BaseSpec<TModel>`
Adds method for paging

```cs
public BaseSpec<TModel> EnablePaging(int skip, int take);
```

Wich one gives you opportunity to easily use your own values

```cs
var spec = new BaseSpec<Entity>().EnablePaging(10, 20);
```

Or you could write your own extension method using this one to use your own pagination filters

Example:

```cs
public static BaseSpec<T> EnablePaginationFilter<T>(this BaseSpec<T> spec, YourPaginationFilter filter)
{
    return spec.EnablePaging(filter.Skip, filter.Take);
}
```
---

Also it provides ExceptBy method with two overloads, which accepts whole models or only values you need to except.

```cs
public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TModel> models, Expression<Func<TModel, TKey>> selector);
public BaseSpec<TModel> ExceptBy<TKey>(IEnumerable<TKey> values, Expression<Func<TModel, TKey>> selector);
```

Example:

```cs
var entitiesToExcept = ...;
var spec = new SomeEntitySpec().ExceptBy(entitiesToExcept, entity => entity.Name);
```

____

### Class `SelectSpec<TModel, TResult>` 
Contains constructor which applied in extension method

```cs
Specification<TModel, TResult> Select<TModel, TResult>(this Specification<TModel> spec, Expression<Func<TModel, TResult>> selector)
```

Which one gives you an opportunity to make Select from already existing specs

```cs
var spec = new SomeEntitySpec().Select(entity => entity.Name);
```

____


Also this package contains an `SafeDeleteSpecAttribute`. For use it you need next package


## Package SpecificationExtensions.Generators
Provides source code generation of `SafeDeleteSpec` class

If you have a concept of safe deletion in your project (marking entity as deleted with boolean flag), 
you might want to have search specification that sets default predicate for it

Class `SafeDeleteSpec` needs an base class or an interface wich you use to provide your boolean flag and name of this field.
It automatically inherits `BaseSpec` class

All you need to have this class is create an empty `partial` class with `SafeDeleteSpecAttribute`

```cs
public interface ISafeDelete
{
    bool IsDeleted { get; set; }
}

[SafeDeleteSpec(typeof(ISafeDelete), nameof(ISafeDelete.IsDeleted))]
public partial class SafeDeleteSpec<TModel>
{
}
```

Pass to constuctor `typeof()` expression of your class/interface and `nameof` expression of required field

**Warning! `nameof()` expression is required. Don't try to write string literal there!**

This will generate a class for you. Which by default searches only for entities with deletion status `false`

But if you need to choose other variant of predicate you can call one of the method that class has

```cs
public SafeDeleteSpec<TModel> LoadAll();
public SafeDeleteSpec<TModel> LoadOnlyDeleted();
public SafeDeleteSpec<TModel> LoadOnlyNotDeleted();
```

And you can use it right in point of use

Also you can combine all of this extensions

```cs
public class SafeDeleteEntityByNameSpec : SafeDeleteSpec<SafeDeleteEntity>
{
    public SafeDeleteEntityByNameSpec(string containingName)
    {
        Query
          .Where(entity => entity.Name.Contains(containingName));
    }
}

var entitiesToExcept = ...;
var spec = new SafeDeleteEntityByNameSpec(predicateName)
                   .LoadAll()
                   .ExceptBy(entitiesToExcept, entity => entity.Name)
                   .Select(entity => entity.Name);
```

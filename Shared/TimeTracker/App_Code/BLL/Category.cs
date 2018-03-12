using System;
using System.Collections.Generic;
using ASPNET.StarterKit.DataAccessLayer;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public class Category {
    /*** FIELD PRIVATE ***/
    private decimal _ActualDuration;
    private string _Abbreviation;
    private int _Id;
    private decimal _EstimateDuration;
    private string _Name;
    private int _ProjectId;

    /*** CONSTRUCTOR ***/
    public Category(string name, int projectId)
      : this(string.Empty, DefaultValues.GetDurationMinValue(), DefaultValues.GetCategoryIdMinValue(), DefaultValues.GetDurationMinValue(), name, projectId) {
    }

    public Category(string abbreviation, decimal estimateDuration, string name, int projectId)
      : this(abbreviation, DefaultValues.GetDurationMinValue(), DefaultValues.GetCategoryIdMinValue(), estimateDuration, name, projectId) {
    }

    public Category(string abbreviation, decimal actualDuration, int id, decimal estimateDuration, string name, int projectId) {
      if (String.IsNullOrEmpty(name))
        throw (new NullReferenceException("name"));

      if (projectId <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("projectId"));

      _Abbreviation = abbreviation;
      _ActualDuration = actualDuration;
      _Id = id;
      _EstimateDuration = estimateDuration;
      _Name = name;
      _ProjectId = projectId;
    }

    /*** PROPERTIES ***/
    public decimal ActualDuration {
      get { return _ActualDuration; }
    }

    public string Abbreviation {
      get {
        if (String.IsNullOrEmpty(_Abbreviation))
          return string.Empty;
        else
          return _Abbreviation;
      }
      set { _Abbreviation = value; }
    }

    public int Id {
      get { return _Id; }
    }

    public decimal EstimateDuration {
      get { return _EstimateDuration; }
      set { _EstimateDuration = value; }
    }

    public string Name {
      get {
        if (String.IsNullOrEmpty(_Name))
          return string.Empty;
        else
          return _Name;
      }
      set { _Name = value; }
    }

    public int ProjectId {
      get { return _ProjectId; }
      set { _ProjectId = value; }
    }

    /*** METHODS  ***/
    public bool Delete() {
      if (this.Id > DefaultValues.GetCategoryIdMinValue()) {
        DataAccess DALLayer = DataAccessHelper.GetDataAccess();
        return DALLayer.DeleteCategory(this.Id);
      }
      else
        return false;
    }

    public bool Save() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      if (Id <= DefaultValues.GetCategoryIdMinValue()) {
        int TempId = DALLayer.CreateNewCategory(this);
        if (TempId > DefaultValues.GetCategoryIdMinValue()) {
          _Id = TempId;
          return true;
        }
        else
          return false;
      }
      else
        return (DALLayer.UpdateCategory(this));
    }

    /*** METHOD STATIC ***/
    public static bool DeleteCategory(int id, int original_Id) {
      if (original_Id <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("original_Id"));

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.DeleteCategory(original_Id));
    }

    public static List<Category> GetAllCategories() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetAllCategories());
    }

    public static Category GetCategoryByCategoryId(int Id) {
      if (Id <= DefaultValues.GetCategoryIdMinValue())
        return (null);

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetCategoryByCategoryId(Id));
    }

    public static List<Category> GetCategoriesByProjectId(int projectId) {
      if (projectId <= DefaultValues.GetProjectIdMinValue())
        return (new List<Category>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetCategoriesByProjectId(projectId));
    }

    public static Category GetCategoryByCategoryNameandProjectId(string categoryName, int projectId) {
      if (projectId <= DefaultValues.GetProjectIdMinValue() || String.IsNullOrEmpty(categoryName))
        return (null);

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetCategoryByCategoryNameandProjectId(categoryName, projectId));

    }


    public static List<Category> GetCategoriesByProjectIds(string ProjectIds) {

      if (String.IsNullOrEmpty(ProjectIds))
        return (new List<Category>());

      char[] separator = new char[] { ',' };
      string[] substrings = ProjectIds.Split(separator);// ,true);
      List<Category> list = new List<Category>();

      foreach (string str in substrings) {
        int id = Convert.ToInt32(str);
        List<Category> tempList = Category.GetCategoriesByProjectId(id);
        foreach (Category category in tempList) {
          list.Add(category);
        }
      }
      return list;
    }

    public static bool UpdateCategory(string Abbreviation, int original_Id, decimal EstimateDuration, string Name/*, int id*/) {
      if (String.IsNullOrEmpty(Name))
        throw (new NullReferenceException("Name"));


      Category updateCategory = Category.GetCategoryByCategoryId(original_Id);
      if (updateCategory != null) {
        updateCategory.Abbreviation = Abbreviation;
        updateCategory.EstimateDuration = EstimateDuration;
        updateCategory.Name = Name;
        return (updateCategory.Save());
      }
      else
        return false;
    }
  }
}

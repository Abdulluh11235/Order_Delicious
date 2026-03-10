namespace Domain;

public class Result<T>
{
   public bool IsSuccess { get; init; }
   public T? Value { get; init; } // carries DTOs and Services Create It 
   public string? ErrorMessage { get; init; }

    public Result(bool isSuccess=true,string? errorMessage=null)
    {
        isSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}
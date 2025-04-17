import java.io.*;
import java.math.*;
import java.security.*;
import java.text.*;
import java.util.*;
import java.util.concurrent.*;
import java.util.function.*;
import java.util.regex.*;
import java.util.stream.*;
import static java.util.stream.Collectors.joining;
import static java.util.stream.Collectors.toList;

public class TimeConversion
{
    class Result
    {
        /*
         * Complete the 'timeConversion' function below.
         *
         * The function is expected to return a STRING.
         * The function accepts STRING s as parameter.
         */

        public static list<String> timeConversion(String s)
        {
            list<String> temp = arrays.asList(s);
            // Write your code here
            return temp;
        }
    }
    public static void main(String[] args) throws IOException
    {
        String s = bufferedReader.readLine();

        list<String> result = Result.timeConversion(s);

        System.out.println(result);
    }
}

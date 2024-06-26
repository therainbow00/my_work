#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>

#define K	1024

/** (2pts)
 * Make a program that accepts two numbers from the command line, an integer
 * and a base.  Convert the first number to a string in the given base, then
 * output the string.  Base may range from 2 to 16.  You may use atoi(), but
 * you may not use %d,%x or %o in printf or sprintf to directly print any
 * numbers.
 *
 * Example input/output:
 * ./p4 101 2
 * 1100101
 * ./p4 65535 16
 * FFFF
 */

int main(int argc, char *argv[])
{
  if (argc < 3) {
    printf("Usage: p4 <integer> <base>\n");
    return 0;
  }

  int num = atoi(argv[1]), base = atoi(argv[2]);
  char str[128], *digits[] = "0123456789ABCDEF";

  int i = 127;
  str[i--] = '\0';

  do {
    str[i--] = digits[num % base];
    num = num / base;
  } while (num > 0);
  i++;

  printf("%s\n", str+i);

  return 0;
}

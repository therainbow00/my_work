CC=gcc -ggdb
AS=nasm -felf64 -g -Fdwarf

hello: hello.o
	$(CC) -o hello hello.o

hello.o: hello.s
	$(AS) hello.s

clean:
	rm -f *.o hello

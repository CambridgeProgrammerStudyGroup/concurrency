#include <stdio.h>
#include <pthread.h>

int x = 0;
int y = 0;

void* increment(void* _)
{
    while (1)
    {
        x++;
        y++;
    }
    
    return 0;
}

void* check(void* _)
{
    while (1)
    {
        if (x < y)
        {
            printf("Oh, dear!\n");
        }
    }
    
    return 0;
}

int main(int argc, const char * argv[]) {
    pthread_t t1, t2;
    
    pthread_create(&t1, 0, increment, 0);
    pthread_create(&t2, 0, check, 0);
    
    pthread_join(t1, 0);
    pthread_join(t2, 0);
    
    return 0;
}

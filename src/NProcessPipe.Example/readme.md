# Examples #

-------

### Basic pipeline ###
A basic pipeline process which executes in order

### Process with custom context ###
A process with a custom context showing how data can be stuffed in the context and shared between all rows

### Mixed yielding pipeline ###
A process which mixes streaming operations (per row) which return using the yielding keyword with synchronous operations (per operation) which return all the data at the completion of their execution 

The output of the pipeline is as follows:

1. Operation 1 running with message 'First Row'
2. Operation 2 running with message 'First Row'
3. Operation 1 running with message 'Second Row'
4. Operation 2 running with message 'Second Row'
5. Operation 3 running with message 'First Row'
6. Operation 3 running with message 'Second Row'

### Add and remove ###
Yet to be done

### Product import ###
A simple process showing how you would load data from a flat model into some sort of database

Process operations:

- Creates and stores product categories which are not in the database
- Finds the matching product category GUID for the product and then store the product in the database

### Send Orders ###
Yet to be done